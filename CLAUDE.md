# SuiteCoreBackend — Contexto del Proyecto

## Stack Tecnológico

- **.NET 8.0** — ASP.NET Core Web API
- **Autenticación:** JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer v8.0.27)
- **LDAP/Active Directory:** System.DirectoryServices.Protocols v10.0.9
- **Sesiones RADIUS:** Flexinets.Radius.Core + Flexinets.Radius.RadiusClient (Accounting Start/Stop) — suspendido temporalmente, sesiones manejadas por DB propia
- **ORM/DB:** Entity Framework Core 8.0.8 + Npgsql.EntityFrameworkCore.PostgreSQL 8.0.4 (PostgreSQL en `172.16.20.15`)
- **Documentación API:** Swagger/OpenAPI via Swashbuckle.AspNetCore v6.6.2

## Arquitectura

N-Tier (capas) con inyección de dependencias:

```
Controllers  →  Services (Interfaces)  →  Infrastructure (Repositories)
                     ↑                            ↑
              Models / DTOs / Settings       SCDbContext (PostgreSQL)
                     ↑
                  Helpers/
```

Todas las interfaces de servicio y repositorio se registran como **Scoped** en el contenedor DI.

## Estructura de Directorios

```
SuiteCoreBackend/
├── Controllers/                    # Endpoints HTTP
│   ├── AuthController.cs           # Login, Logout, /me, /admin
│   ├── MonitoringController.cs     # Tipos de dispositivo, paneles Grafana
│   └── NetboxController.cs         # Regiones e IP addresses de Netbox
├── DTOs/
│   ├── Auth/                       # LoginRequestDto, LoginResponseDto, LdapUserDto
│   ├── Monitoring/                 # DeviceTypeDto, GrafanaPanelDto
│   └── Netbox/                     # CreateNetboxRegionDto, NetboxIpAddressDto, etc.
├── Helpers/
│   └── DateTimeHelper.cs           # Utilidad de zona horaria (Perú)
├── Models/Entities/                # Entidades de dominio
│   ├── LdapUser.cs
│   ├── GrafanaPanel.cs             # Tabla grafanapanels en PostgreSQL
│   └── UserActivity.cs             # Tabla useractivities — registro de sesiones web
├── Services/
│   ├── Interfaces/                 # Contratos de servicio
│   └── Implementations/            # Lógica de negocio
│   └── Monitoring/                 # GrafanaService, LibreNmsService
├── Infrastructure/
│   ├── Context/SCDbContext.cs      # DbContext con GrafanaPanels y UserActivities
│   ├── Interfaces/                 # IGrafanaRepository, IUserActivityRepository
│   └── Implementations/            # GrafanaRepository, UserActivityRepository
├── Settings/                       # Clases de configuración fuertemente tipadas
└── Program.cs                      # Startup, DI, middleware pipeline
```

## Endpoints

| Método | Ruta                          | Auth                   | Descripción                          |
|--------|-------------------------------|------------------------|--------------------------------------|
| POST   | /api/auth/login               | Anónimo                | Login LDAP → JWT + registro sesión   |
| POST   | /api/auth/logout              | [Authorize]            | Cierra sesión                        |
| GET    | /api/auth/me                  | [Authorize]            | Datos del usuario autenticado        |
| GET    | /api/auth/admin               | [Authorize(Rol=Admin)] | Solo Administradores                 |
| GET    | /api/monitoring/device-types  | (temporalmente abierto)| Tipos de dispositivo LibreNMS        |
| GET    | /api/monitoring/grafana-panels| (temporalmente abierto)| Paneles Grafana desde DB             |
| GET    | /api/netbox/regions           | (temporalmente abierto)| Obtener regiones desde Netbox        |
| GET    | /api/netbox/regions/{id}      | (temporalmente abierto)| Obtener región por ID desde Netbox   |
| POST   | /api/netbox/regions           | (temporalmente abierto)| Crear región en Netbox               |
| PATCH  | /api/netbox/regions/{id}      | (temporalmente abierto)| Actualizar región en Netbox          |
| DELETE | /api/netbox/regions/{id}      | (temporalmente abierto)| Eliminar región en Netbox            |
| GET    | /api/netbox/ip-addresses      | (temporalmente abierto)| Obtener direcciones IP desde Netbox  |
| GET    | /weatherforecast              | Ninguna                | Endpoint de prueba (scaffold)        |

## Flujo de Autenticación

```
POST /api/auth/login
  → AuthService.LoginAsync(request, clientIp)
      → LdapAuthService.Authenticate()           # Bind servicio → búsqueda → bind usuario
          # 1. Bind con cuenta de servicio
          # 2. Busca usuario por uid
          # 3. Bind con credenciales del usuario
          # 4. Extrae atributos y grupos → LdapUser
      → generateIdSession()                      # SessionId = GUID 16 chars en AuthService
      → UserActivityRepository.RegisterLogin()   # INSERT en useractivities (PostgreSQL)
          # SessionId, Username, IpAddress, StartedAt, EndedAt, LastActivityAt
      → [RADIUS suspendido] RadiusSessionService.StartSessionAsync()
      → JwtService.GenerateToken(user, sessionId) # JWT HS256 con sessionId como claim
  ← LoginResponseDto { Token, ExpiresAt, User, SessionId }

POST /api/auth/logout
  → Extrae sessionId y username del JWT
  → AuthService.LogoutAsync(sessionId, username)
      → RadiusSessionService.StopSessionAsync()  # Accounting-Stop (aún activo en código)
  ← 200 OK
```

**Nota sobre RADIUS:** RADIUS en la infraestructura del cliente se usa exclusivamente para autenticar accesos a equipos de red y SSH a servidores — no para sesiones de aplicaciones web. Las sesiones del Dashboard se trackean en la tabla `useractivities` de PostgreSQL.

El token JWT incluye los claims: `name`, `givenName`, `department`, `username`, `sessionId`, y roles extraídos del atributo `memberOf` de LDAP.

## Servicios

| Interfaz                | Implementación         | Propósito                                                      |
|-------------------------|------------------------|----------------------------------------------------------------|
| IAuthService            | AuthService            | Orquesta flujo login LDAP → SessionId → UserActivity → JWT    |
| IJwtService             | JwtService             | Genera tokens JWT HS256 (incluye sessionId como claim)         |
| ILdapAuthService        | LdapAuthService        | Autenticación y lectura de atributos LDAP/AD                   |
| IRadiusSessionService   | RadiusSessionService   | Accounting Start/Stop contra RADIUS (suspendido en login)      |
| IGrafanaService         | GrafanaService         | Construye URLs de paneles Grafana consultando DB               |
| ILibreNmsService        | LibreNmsService        | Consulta dispositivos desde LibreNMS vía HTTP                  |
| INetboxService          | NetboxService          | Consulta regiones e IPs desde Netbox vía HTTP                  |

## Repositorios (Infrastructure)

| Interfaz                  | Implementación           | Propósito                                          |
|---------------------------|--------------------------|----------------------------------------------------|
| IGrafanaRepository        | GrafanaRepository        | CRUD sobre tabla `grafanapanels`                   |
| IUserActivityRepository   | UserActivityRepository   | Registro de sesiones web en tabla `useractivities` |

## Modelos Principales

**LdapUser** — resultado de autenticar con LDAP:
- `DisplayName`, `FirstName`, `LastName`, `Email`, `Username`
- `Department`, `Title`
- `Groups: List<string>` — DNs de grupos
- `Roles: List<string>` — extraídos de los CN de grupos

**GrafanaPanel** — entidad DB (`grafanapanels`):
- `Id`, `Name`, `DashboardUid`, `PanelId`

**UserActivity** — entidad DB (`useractivities`), sesiones web activas:
- `SessionId`, `Username`, `IpAddress`
- `StartedAt`, `EndedAt`, `LastActivityAt`

**LdapSettings** (appsettings.json `Ldap:*`):
- `Server`, `Port`, `UseSSL`, `BaseDn`, `ServiceUser`, `ServicePassword`

**JwtSettings** (appsettings.json `Jwt:*`):
- `Key`, `Issuer` (`SuiteCoreApi`), `Audience` (`SuiteCoreFrontend`), `ExpiresInMinutes` (120)

**RadiusSettings** (appsettings.json `Radius:*`):
- `Server` (`172.16.20.12`), `AccountingPort` (1813), `SharedSecret`, `TimeoutMs` (5000)

## Configuración

Los valores sensibles viven en `appsettings.json` (no commitear en prod):
- Clave secreta JWT: `SuiteCoreApi_Jwt_PrivateKey_2026_NOC_Seguridad_ClaveLarga`
- Credenciales de cuenta de servicio LDAP
- IP del servidor LDAP: `172.16.20.17`
- IP del servidor RADIUS: `172.16.20.12`
- Connection string PostgreSQL: `Host=172.16.20.15;Database=SuiteCore;Username=admin;Password=suitecore123$`

Las clases de configuración son: `JwtSettings`, `LdapSettings` y `RadiusSettings`, leídas con el patrón `IOptions<T>`.

## Helpers

**DateTimeHelper** (`Helpers/DateTimeHelper.cs`):
- `GetPeruDateTime()` — retorna `DateTime` en zona horaria `SA Pacific Standard Time` (UTC-5)
- Usado en `AuthService` para registrar `StartedAt` y `LastActivityAt` en hora local Perú

## Middleware Pipeline (Orden)

1. Swagger UI (solo en Development)
2. HTTPS Redirection
3. Authentication
4. Authorization
5. MapControllers

## Convenciones del Proyecto

- Nombres de propiedades en inglés, comentarios en español
- Interfaces de servicio con prefijo `I` en `Services/Interfaces/`
- Interfaces de repositorio con prefijo `I` en `Infrastructure/Interfaces/`
- DTOs agrupados por feature bajo `DTOs/<Feature>/`
- Entidades bajo `Models/Entities/`
- Helpers estáticos bajo `Helpers/`
- Tiempo de vida de servicios y repositorios: `Scoped` por convención
- Validación JWT sin `ClockSkew` (tolerancia cero)
- Columnas de DB en minúsculas (`[Column("name")]`), tablas en minúsculas (`[Table("grafanapanels")]`)

## Estado Actual del Proyecto

- **Listo:** Autenticación LDAP → JWT completa con RBAC
- **Listo:** EF Core configurado con PostgreSQL (`SCDbContext`, migraciones pendientes de ejecutar)
- **Listo:** Registro de sesiones web en tabla `useractivities` (StartedAt, EndedAt, IpAddress)
- **Listo:** Paneles Grafana dinámicos desde tabla `grafanapanels` en DB
- **Suspendido:** RADIUS Accounting-Start en login (código presente pero comentado) — RADIUS es para equipos de red, no sesiones web
- **Activo:** RADIUS Accounting-Stop en logout (pendiente de evaluar si se mantiene)
- **Pendiente:** Ejecutar migraciones EF Core en PostgreSQL
- **Pendiente:** Endpoint para consultar sesiones activas (`useractivities WHERE EndedAt IS NULL`)
- **Pendiente:** Restaurar `[Authorize]` en `MonitoringController` y `NetboxController` (actualmente comentado para pruebas)
- **Scaffolding:** `WeatherForecastController` es código de plantilla — puede eliminarse

## Puertos Locales

| Perfil       | HTTP            | HTTPS           |
|--------------|-----------------|-----------------|
| Dev (dotnet) | localhost:5182  | localhost:7073  |
| IIS Express  | localhost:31819 | localhost:44356 |

Swagger UI disponible en `/swagger` al correr en Development.
