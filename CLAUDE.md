# SuiteCoreBackend — Contexto del Proyecto

## Stack Tecnológico

- **.NET 8.0** — ASP.NET Core Web API
- **Autenticación:** JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer v8.0.27)
- **LDAP/Active Directory:** System.DirectoryServices.Protocols v10.0.9
- **Sesiones RADIUS:** Flexinets.Radius.Core + Flexinets.Radius.RadiusClient (Accounting Start/Stop)
- **Documentación API:** Swagger/OpenAPI via Swashbuckle.AspNetCore v6.6.2
- **ORM/DB:** SCDbContext preparado (Entity Framework, sin configurar aún)

## Arquitectura

N-Tier (capas) con inyección de dependencias:

```
Controllers  →  Services (Interfaces)  →  Infrastructure (Context)
                     ↑
              Models / DTOs / Settings
```

Todas las interfaces de servicio se registran como **Scoped** en el contenedor DI.

## Estructura de Directorios

```
SuiteCoreBackend/
├── Controllers/            # Endpoints HTTP
│   └── AuthController.cs   # Login, /me, /admin
├── DTOs/Auth/              # Contratos de entrada/salida de la API
├── Models/Entities/        # Entidades de dominio
├── Services/               # Lógica de negocio
│   └── Interfaces/         # Contratos de servicio
├── Settings/               # Clases de configuración fuertemente tipadas
├── Infrastructure/Context/ # SCDbContext (placeholder, aún vacío)
└── Program.cs              # Startup, DI, middleware pipeline
```

## Endpoints

| Método | Ruta              | Auth                    | Descripción                    |
|--------|-------------------|-------------------------|--------------------------------|
| POST   | /api/auth/login   | Anónimo                 | Login con usuario/contraseña   |
| POST   | /api/auth/logout  | [Authorize]             | Cierra sesión (RADIUS Stop)    |
| GET    | /api/auth/me      | [Authorize]             | Datos del usuario autenticado  |
| GET    | /api/auth/admin   | [Authorize(Rol=Admin)]  | Solo Administradores           |
| GET    | /weatherforecast  | Ninguna                 | Endpoint de prueba (scaffold)  |

## Flujo de Autenticación

```
POST /api/auth/login
  → AuthService.LoginAsync(request, clientIp)
      → LdapAuthService.Authenticate()        # Bind de servicio → búsqueda → bind de usuario
          # 1. Bind con cuenta de servicio
          # 2. Busca usuario por uid
          # 3. Bind con credenciales del usuario
          # 4. Extrae atributos y grupos → LdapUser
      → RadiusSessionService.StartSessionAsync()  # Accounting-Start → RADIUS puerto 1813
          # Genera SessionId (GUID 16 chars)
          # Si RADIUS falla, loguea warning pero NO bloquea el login
      → JwtService.GenerateToken(user, sessionId)  # JWT HS256 con sessionId como claim
  ← LoginResponseDto { Token, ExpiresAt, User, SessionId }

POST /api/auth/logout
  → Extrae sessionId y username del JWT
  → AuthService.LogoutAsync(sessionId, username)
      → RadiusSessionService.StopSessionAsync()   # Accounting-Stop → RADIUS puerto 1813
  ← 200 OK
```

El token JWT incluye los claims: `name`, `givenName`, `department`, `username`, `sessionId`, y roles extraídos del atributo `memberOf` de LDAP.

## Servicios

| Interfaz                | Implementación         | Propósito                                               |
|-------------------------|------------------------|---------------------------------------------------------|
| IAuthService            | AuthService            | Orquesta el flujo login LDAP → RADIUS → JWT             |
| IJwtService             | JwtService             | Genera tokens JWT HS256 (incluye sessionId como claim)  |
| ILdapAuthService        | LdapAuthService        | Autenticación y lectura de atributos LDAP/AD            |
| IRadiusSessionService   | RadiusSessionService   | Accounting Start/Stop contra servidor RADIUS            |
| ITestUserService        | TestUserService        | Usuarios de prueba hardcodeados (no usado en prod)      |

## Modelos Principales

**LdapUser** — resultado de autenticar con LDAP:
- `DisplayName`, `FirstName`, `LastName`, `Email`, `Username`
- `Department`, `Title`
- `Groups: List<string>` — DNs de grupos
- `Roles: List<string>` — extraídos de los CN de grupos

**LdapSettings** (appsettings.json `Ldap:*`):
- `Server`, `Port`, `UseSSL`, `BaseDn`, `ServiceUser`, `ServicePassword`

**JwtSettings** (appsettings.json `Jwt:*`):
- `Key`, `Issuer` (`SuiteCoreApi`), `Audience` (`SuiteCoreFrontend`), `ExpiresInMinutes` (120)

**RadiusSettings** (appsettings.json `Radius:*`):
- `Server` (IP del servidor RADIUS: `172.16.20.12`), `AccountingPort` (1813), `SharedSecret`, `TimeoutMs` (5000)

## Configuración

Los valores sensibles viven en `appsettings.json` (no commitear en prod):
- Clave secreta JWT: `SuiteCoreApi_Jwt_PrivateKey_2026_NOC_Seguridad_ClaveLarga`
- Credenciales de cuenta de servicio LDAP
- IP del servidor LDAP: `172.16.20.17`
- IP del servidor RADIUS: `172.16.20.12`

Las clases de configuración son: `JwtSettings`, `LdapSettings` y `RadiusSettings`, leídas con el patrón `IOptions<T>`.

## Usuarios de Prueba (TestUserService)

Solo para desarrollo local — no están integrados en el flujo principal:

| Email                       | Password | Rol           | Activo |
|-----------------------------|----------|---------------|--------|
| admin@suitecore.com         | 123456   | Administrador | true   |
| asesor@suitecore.com        | 123456   | Asesor        | true   |
| inactivo@suitecore.com      | 123456   | Asesor        | false  |

## Middleware Pipeline (Orden)

1. Swagger UI (solo en Development)
2. HTTPS Redirection
3. Authentication
4. Authorization
5. MapControllers

## Convenciones del Proyecto

- Nombres de propiedades en inglés, comentarios en español
- Interfaces con prefijo `I` en carpeta `Services/Interfaces/`
- DTOs agrupados por feature bajo `DTOs/<Feature>/`
- Entidades bajo `Models/Entities/`
- Tiempo de vida de servicios: `Scoped` por convención
- Validación JWT sin `ClockSkew` (tolerancia cero)

## Estado Actual del Proyecto

- **Listo:** Autenticación LDAP → JWT completa con RBAC
- **Listo:** Sesiones RADIUS — Accounting Start en login, Stop en logout. RADIUS no bloquea el login si falla.
- **Pendiente:** Configurar EF Core en `SCDbContext`, agregar entidades de negocio, migraciones
- **Scaffolding:** `WeatherForecastController` es código de plantilla — puede eliminarse

## Puertos Locales

| Perfil       | HTTP            | HTTPS           |
|--------------|-----------------|-----------------|
| Dev (dotnet) | localhost:5182  | localhost:7073  |
| IIS Express  | localhost:31819 | localhost:44356 |

Swagger UI disponible en `/swagger` al correr en Development.
