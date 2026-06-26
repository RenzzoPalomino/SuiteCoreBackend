# SuiteCoreBackend — Contexto del Proyecto

## Stack Tecnológico

- **.NET 8.0** — ASP.NET Core Web API
- **Autenticación:** JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer v8.0.27)
- **LDAP/Active Directory:** System.DirectoryServices.Protocols v10.0.9
- **Sesiones RADIUS:** Flexinets.Radius.Core + Flexinets.Radius.RadiusClient (Accounting Start/Stop) — suspendido en login, activo en logout
- **ORM/DB:** Entity Framework Core 8.0.8 + Npgsql.EntityFrameworkCore.PostgreSQL 8.0.4 (PostgreSQL en `172.16.20.15`)
- **Mapeo de objetos:** AutoMapper
- **Documentación API:** Swagger/OpenAPI via Swashbuckle.AspNetCore v6.6.2

## Arquitectura

N-Tier (capas) con inyección de dependencias:

```
Controllers  →  Services (Interfaces)  →  Infrastructure (Repositories)
                     ↑                            ↑
              Models / DTOs / Settings       SCDbContext (PostgreSQL)
                     ↑
               Helpers / Enums
```

Todas las interfaces de servicio y repositorio se registran como **Scoped** en el contenedor DI.
Servicios HTTP externos (`OxidizedService`, `LibreNmsService`, `NetboxService`) se registran con `AddHttpClient<>`.

## Estructura de Directorios

```
SuiteCoreBackend/
├── Controllers/
│   ├── AuthController.cs           # Login, Logout, /me, /admin
│   ├── MonitoringController.cs     # LibreNMS, Grafana, Netbox
│   ├── OxidizedController.cs       # Dispositivos, versiones y backups Oxidized
│   └── PermissionController.cs     # Menús por rol (JWT claim)
│   ├── MonitoringController.cs     # Tipos de dispositivo, paneles Grafana
│   └── NetboxController.cs         # Regiones e IP addresses de Netbox
├── DTOs/
│   ├── Auth/                       # LoginRequestDto, LoginResponseDto, LdapUserDto, UserSessionDto
│   ├── Menu/                       # MenuBlockDto, MenuItemDto
│   ├── Monitoring/                 # DeviceTypeDto, GrafanaPanelDto, Netbox*Dto
│   └── Oxidized/                   # OxidizedDeviceDto, OxidizedBackupDto, OxidizedVersionDto
├── Enums/
│   ├── AccountingStatus.cs         # Start = 1, Stop = 2
│   ├── DateValues.cs               # UTC_MINUS_FIVE = -5
│   └── RoleGroups.cs               # Constantes de gidNumber por rol
│   ├── Auth/                       # LoginRequestDto, LoginResponseDto, LdapUserDto
│   ├── Monitoring/                 # DeviceTypeDto, GrafanaPanelDto
│   └── Netbox/                     # CreateNetboxRegionDto, NetboxIpAddressDto, etc.
├── Helpers/
│   ├── DateTimeHelper.cs           # GetPeruDateTime() — UTC-5
│   └── OxidizeHelper.cs            # ConvertToEpoch(), NormalizeOxidizedConfig()
├── Models/Entities/
│   ├── LdapSettings.cs             # Clases LdapUser y LdapRole (no mapeadas a BD)
│   ├── GrafanaPanel.cs             # Tabla grafanapanels
│   ├── UserActivity.cs             # Tabla useractivities
│   ├── MenuBlock.cs                # Tabla session.menu_blocks
│   ├── Menu.cs                     # Tabla session.menus
│   ├── RoleMenu.cs                 # Tabla session.role_menus (PK compuesta)
│   └── NetBox.cs                   # Modelos de respuesta Netbox API
├── Services/
│   ├── Interfaces/                 # IAuthService, IJwtService, ILdapAuthService,
│   │                               # IRadiusSessionService, IGrafanaService,
│   │                               # ILibreNmsService, INetboxService,
│   │                               # IOxidizedService, IMenuService
│   └── Implementations/            # Implementaciones de cada interfaz
├── Infrastructure/
│   ├── Context/SCDbContext.cs      # DbContext — GrafanaPanels, UserActivities,
│   │                               # MenuBlocks, Menus, RoleMenus
│   ├── Interfaces/                 # IGrafanaRepository, IUserActivityRepository,
│   │                               # IMenuRepository
│   └── Implementations/            # GrafanaRepository, UserActivityRepository,
│                                   # MenuRepository
├── Settings/
│   ├── JwtSettings.cs
│   ├── LdapSettings.cs
│   ├── RadiusSettings.cs
│   ├── OxidizedSettings.cs
│   └── AutoMapperProfile.cs        # Mapeos LdapUser↔LdapUserDto, Netbox*
└── Program.cs                      # Startup, DI, middleware pipeline
```

## Endpoints

### Auth — `/api/auth`
| Método | Ruta      | Auth                              | Descripción                        |
|--------|-----------|-----------------------------------|------------------------------------|
| POST   | `/login`  | Anónimo                           | Login LDAP → JWT + registro sesión |
| POST   | `/logout` | `[Authorize]`                     | Cierra sesión                      |
| GET    | `/me`     | `[Authorize]`                     | Datos del usuario autenticado      |
| GET    | `/admin`  | `[Authorize(Roles = "5101,5103")]` | Solo Network Admin e IT Supervisor |

### Permission — `/api/permission`
| Método | Ruta     | Auth          | Descripción                               |
|--------|----------|---------------|-------------------------------------------|
| GET    | `/Menus` | `[Authorize]` | Menús accesibles según gidNumber del JWT  |

### Monitoring — `/api/monitoring`
| Método | Ruta                    | Auth | Descripción                        |
|--------|-------------------------|------|------------------------------------|
| GET    | `/device-types`         | —    | Tipos de dispositivo LibreNMS      |
| GET    | `/grafana-panels`       | —    | Paneles Grafana desde DB           |
| GET    | `/netbox-regions`       | —    | Lista regiones Netbox              |
| GET    | `/netbox-regions/{id}`  | —    | Detalle de región Netbox           |
| POST   | `/netbox-regions`       | —    | Crea región en Netbox              |
| PATCH  | `/netbox-regions/{id}`  | —    | Actualiza región en Netbox         |
| DELETE | `/netbox-regions/{id}`  | —    | Elimina región en Netbox           |
| GET    | `/netbox-ip-addresses`  | —    | Lista IPs desde Netbox             |

### Oxidized — `/api/oxidized`
| Método | Ruta                            | Auth          | Descripción                        |
|--------|---------------------------------|---------------|------------------------------------|
| GET    | `/devices`                      | `[Authorize]` | Lista dispositivos Oxidized        |
| GET    | `/devices/{deviceName}/versions`| `[Authorize]` | Historial de versiones             |
| GET    | `/devices/{deviceName}/backup`  | `[Authorize]` | Backup de configuración            |

## Flujo de Autenticación

```
POST /api/auth/login
  → AuthService.LoginAsync(request, clientIp)
      → LdapAuthService.Authenticate()           # Bind servicio → búsqueda → bind usuario
      → generateIdSession()                      # SessionId = GUID 16 chars
      → UserActivityRepository.RegisterLogin()   # INSERT en useractivities
      → [RADIUS suspendido] RadiusSessionService.StartSessionAsync()
      → JwtService.GenerateToken(user, sessionId)
  ← LoginResponseDto { Token, ExpiresAt, User, SessionId }

POST /api/auth/logout
  → AuthService.LogoutAsync(sessionId, username)
      → RadiusSessionService.StopSessionAsync()  # Accounting-Stop (activo)
  ← 200 OK
```

### Claims del JWT
| Claim              | Valor                    |
|--------------------|--------------------------|
| `ClaimTypes.Name`  | DisplayName              |
| `ClaimTypes.GivenName` | FirstName            |
| `ClaimTypes.Role`  | GidNumber (del usuario)  |
| `department`       | Department               |
| `username`         | Username (uid)           |
| `sessionId`        | SessionId                |

**RBAC:** `[Authorize(Roles = "5101,5103")]` valida contra el claim `Role` = GidNumber.

## Roles (RoleGroups.cs)

| Constante        | GidNumber | Descripción         |
|------------------|-----------|---------------------|
| `ALL_USER`       | 5000      | Usuarios generales  |
| `NOC`            | 5100      | Operadores NOC      |
| `NETWORK_ADMIN`  | 5101      | Administradores red |
| `SECURITY_TEAM`  | 5102      | Equipo seguridad    |
| `IT_SUPERVISOR`  | 5103      | Supervisores TI     |

## Servicios

| Interfaz              | Implementación       | Registro DI    | Propósito                                               |
|-----------------------|----------------------|----------------|---------------------------------------------------------|
| IAuthService          | AuthService          | Scoped         | Orquesta login LDAP → SessionId → UserActivity → JWT   |
| IJwtService           | JwtService           | Scoped         | Genera tokens JWT HS256                                 |
| ILdapAuthService      | LdapAuthService      | Scoped         | Autenticación LDAP, GetRoles(), GetUsersByGid()         |
| IRadiusSessionService | RadiusSessionService | Scoped         | Accounting Start/Stop RADIUS                            |
| IGrafanaService       | GrafanaService       | Scoped         | Construye URLs de paneles Grafana desde DB              |
| ILibreNmsService      | LibreNmsService      | AddHttpClient  | Consulta tipos de dispositivo LibreNMS                  |
| INetboxService        | NetboxService        | AddHttpClient  | CRUD regiones e IPs en Netbox                           |
| IOxidizedService      | OxidizedService      | AddHttpClient  | Dispositivos, versiones y backups de Oxidized           |
| IMenuService          | MenuService          | Scoped         | Obtiene menús accesibles por gidNumbers del usuario     |

### Métodos clave de ILdapAuthService
```csharp
LdapUser? Authenticate(string username, string password)
List<LdapRole> GetRoles()                        // Busca grupos LDAP (cn, gidNumber, description)
List<LdapUser> GetUsersByGid(string gidNumber)   // Primarios (gidNumber en usuario) + suplementarios (memberUid en grupo)
```

## Repositorios (Infrastructure)

| Interfaz                | Implementación          | Propósito                                              |
|-------------------------|-------------------------|--------------------------------------------------------|
| IGrafanaRepository      | GrafanaRepository       | Lectura de tabla `grafanapanels`                       |
| IUserActivityRepository | UserActivityRepository  | Registro de sesiones web en `useractivities`           |
| IMenuRepository         | MenuRepository          | Menús por gidNumber, asignación/desasignación rol-menú |

### Métodos de IMenuRepository
```csharp
Task<List<MenuBlock>> GetMenusByGids(IEnumerable<string> gidNumbers)  // Filtra por rol + públicos
Task<List<MenuBlock>> GetAllMenus()
Task<List<RoleMenu>>  GetRoleMenus()
Task AssignMenuToRole(string gidNumber, int menuId)
Task RemoveMenuFromRole(string gidNumber, int menuId)
```

## Modelos / Entidades

### Entidades de Base de Datos (EF Core)

| Entidad     | Tabla                   | Descripción                              |
|-------------|-------------------------|------------------------------------------|
| GrafanaPanel| `grafanapanels`         | Id, Name, DashboardUid, PanelId          |
| UserActivity| `useractivities`        | SessionId, Username, IpAddress, StartedAt, EndedAt, LastActivityAt |
| MenuBlock   | `session.menu_blocks`   | Id, Name, Order, Active                  |
| Menu        | `session.menus`         | Id, BlockId, Name, Slug, Order, IsPublic, Active |
| RoleMenu    | `session.role_menus`    | GidNumber + MenuId (PK compuesta)        |

### Modelos LDAP (sin mapeo a BD)

**LdapUser:** DisplayName, FirstName, LastName, Username, UidNumber, GidNumber, Department, Title, Groups, Roles

**LdapRole:** Id (gidNumber), Name (cn), Description

## DTOs

| Carpeta     | DTOs                                                                          |
|-------------|-------------------------------------------------------------------------------|
| Auth/       | LoginRequestDto, LoginResponseDto, LdapUserDto, UserSessionDto                |
| Menu/       | MenuBlockDto `{ Block, Order, Menus }`, MenuItemDto `{ Id, Name, Slug }`      |
| Monitoring/ | DeviceTypeDto, GrafanaPanelDto, NetboxRegionDto, NetboxRegionDetailDto, CreateNetboxRegionDto, UpdateNetboxRegionDto, NetboxIpAddressDto |
| Oxidized/   | OxidizedDeviceDto, OxidizedBackupDto, OxidizedVersionDto                     |

## Integaciones Externas

| Sistema    | IP/URL           | Auth          | Config key en appsettings |
|------------|------------------|---------------|---------------------------|
| LDAP       | 172.16.20.17:389 | Bind user/pass| `Ldap:`                   |
| RADIUS     | 172.16.20.12:1813| Shared Secret | `Radius:`                 |
| Oxidized   | 172.16.20.13     | Basic Auth    | `Oxidized:`               |
| LibreNMS   | 172.16.20.10     | X-Auth-Token  | `LibreNMS:`               |
| Grafana    | 172.16.20.10:3000| Solo URL      | `Grafana:`                |
| Netbox     | 172.16.20.11     | Bearer Token  | `Netbox:`                 |
| PostgreSQL | 172.16.20.15     | User/Pass     | `ConnectionStrings:`      |

## Settings (IOptions<T>)

| Clase           | Sección appsettings |
|-----------------|---------------------|
| JwtSettings     | `Jwt:`              |
| LdapSettings    | `Ldap:`             |
| RadiusSettings  | `Radius:`           |
| OxidizedSettings| `Oxidized:`         |

## AutoMapper (AutoMapperProfile.cs)

```csharp
LdapUser           ↔ LdapUserDto
NetboxRegionResult ↔ NetboxRegionDto
NetboxRegionResult ↔ NetboxRegionDetailDto
NetboxIpAddressResult ↔ NetboxIpAddressDto
NetboxStatusResult ↔ NetboxStatusDto
```

## Middleware Pipeline (Orden)

1. Swagger UI (solo Development)
2. HTTPS Redirection
3. CORS (`AllowedOrigins`: localhost:5173)
4. Authentication
5. Authorization
6. MapControllers

## Convenciones del Proyecto

- Nombres de propiedades en inglés, comentarios en español
- Interfaces de servicio con prefijo `I` en `Services/Interfaces/`
- Interfaces de repositorio con prefijo `I` en `Infrastructure/Interfaces/`
- DTOs agrupados por feature bajo `DTOs/<Feature>/`
- Entidades bajo `Models/Entities/`
- Helpers estáticos bajo `Helpers/`
- Tiempo de vida: Scoped para servicios y repositorios, AddHttpClient para clientes HTTP externos
- Validación JWT sin `ClockSkew` (tolerancia cero)
- Columnas de DB en snake_case (`[Column("block_id")]`), tablas en minúsculas con schema cuando aplica (`[Table("menu_blocks", Schema = "session")]`)
- PK compuesta con `[PrimaryKey(nameof(A), nameof(B))]` (EF8)

## Estado Actual del Proyecto

- **Listo:** Autenticación LDAP → JWT completa con RBAC por gidNumber
- **Listo:** Registro de sesiones web en tabla `useractivities`
- **Listo:** Paneles Grafana dinámicos desde tabla `grafanapanels`
- **Listo:** Integración Oxidized — dispositivos, versiones, backups
- **Listo:** Integración Netbox — regiones e IPs (CRUD)
- **Listo:** Integración LibreNMS — tipos de dispositivo
- **Listo:** LDAP GetRoles() y GetUsersByGid() — búsqueda por gidNumber primario + memberUid
- **Listo:** Entidades EF Core para sistema de menús (MenuBlock, Menu, RoleMenu — schema `session`)
- **Listo:** MenuRepository con filtro por gidNumbers + menús públicos
- **Listo:** MenuService.GetMenusForUser() — mapea a MenuBlockDto agrupado
- **En progreso:** PermissionController — GET /api/permission/Menus activo, faltan endpoints de gestión (asignar/desasignar)
- **Suspendido:** RADIUS Accounting-Start en login — RADIUS es para equipos de red, no sesiones web
- **Pendiente:** Restaurar `[Authorize]` en `MonitoringController` (sin auth actualmente)
- **Pendiente:** Endpoints admin de gestión de roles-menús (AssignMenuToRole, RemoveMenuFromRole)

## Puertos Locales

| Perfil       | HTTP            | HTTPS           |
|--------------|-----------------|-----------------|
| Dev (dotnet) | localhost:5182  | localhost:7073  |
| IIS Express  | localhost:31819 | localhost:44356 |

Swagger UI disponible en `/swagger` al correr en Development.
