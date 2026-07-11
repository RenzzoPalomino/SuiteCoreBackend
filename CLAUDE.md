# SuiteCoreBackend — Contexto del Proyecto

## Stack Tecnológico

- **.NET 8.0** — ASP.NET Core Web API
- **Autenticación:** JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer v8.0.27)
- **LDAP/Active Directory:** System.DirectoryServices.Protocols v10.0.9
- **Sesiones RADIUS:** Flexinets.Radius.Core + Flexinets.Radius.RadiusClient (Accounting Start/Stop) — suspendido en login, activo en logout
- **ORM/DB:** Entity Framework Core 8.0.8 + Npgsql.EntityFrameworkCore.PostgreSQL 8.0.4 (PostgreSQL en `172.16.20.15`)
- **Mapeo de objetos:** AutoMapper
- **Documentación API:** Swagger/OpenAPI via Swashbuckle.AspNetCore v6.6.2
- **SSH:** Renci.SshNet 2024.2.0 — usado por WireGuardService para ejecutar `sudo wg show` y leer `/proc/net/dev` en el servidor VPN

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
Servicios HTTP externos (`OxidizedService`, `LibreNmsService`, `NetboxService`, `TailscaleService`) se registran con `AddHttpClient<>`.
`WireGuardService` usa SSH (no HTTP), se registra como **Scoped**.

## Estructura de Directorios

```
SuiteCoreBackend/
├── Controllers/
│   ├── AuthController.cs                # Login, Logout, /me, /admin
│   ├── LdapController.cs                # CRUD usuarios LDAP (GetByGid, Create, Update, Disable, Enable)
│   ├── MonitoringController.cs          # LibreNMS device types, Grafana panels
│   ├── NetboxController.cs              # CRUD completo Netbox: regions, IPs, vlans, cables,
│   │                                    # sites, devices, racks, manufacturers, device-roles,
│   │                                    # module-type-profiles
│   ├── OxidizedController.cs            # Dispositivos, versiones y backups Oxidized
│   ├── PermissionController.cs          # Menús por rol (JWT claim)
│   ├── AlertController.cs               # Webhooks Grafana y LibreNMS → Telegram
│   ├── NotificationChannelController.cs # CRUD canales Telegram + test
│   └── VpnController.cs                 # WireGuard, Tailscale, Access Policy, Stats
├── DTOs/
│   ├── Auth/                       # LoginRequestDto, LoginResponseDto, LdapUserDto,
│   │                               # UserSessionDto, LdapRoleDto,
│   │                               # CreateLdapUserDto, UpdateLdapUserDto
│   ├── Alert/                      # GrafanaWebhookDto, LibreNmsWebhookDto
│   ├── Menu/                       # MenuBlockDto, MenuItemDto, AssignMenusRequestDto
│   ├── Monitoring/                 # DeviceTypeDto, GrafanaPanelDto
│   ├── Netbox/                     # NetboxRegionDto, NetboxIpAddressDto, NetboxVlanDto,
│   │                               # NetboxCableDto, NetboxSiteDto, NetboxDeviceDto,
│   │                               # NetboxRackDto, NetboxManufacturerDto,
│   │                               # NetboxDeviceRoleDto, NetboxModuleTypeProfileDto
│   │                               # + Create*/Update* variants para cada recurso
│   ├── Notification/               # NotificationChannelDto, CreateNotificationChannelDto,
│   │                               # UpdateNotificationChannelDto, TestNotificationDirectDto
│   ├── Oxidized/                   # OxidizedDeviceDto, OxidizedBackupDto, OxidizedVersionDto
│   └── Vpn/                        # VpnGatewayStatusDto, WireGuardStatusDto, WireGuardPeerDto,
│                                   # WireGuardStatsDto, TailscaleStatusDto, TailscalePeerDto,
│                                   # VpnAccessPolicyDto, AccessDestinationDto
├── Enums/
│   ├── AccountingStatus.cs         # Start = 1, Stop = 2
│   ├── DateValues.cs               # UTC_MINUS_FIVE = -5
│   └── RoleGroups.cs               # Constantes de gidNumber por rol
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
│   │                               # ILibreNmsService, INetboxService, IOxidizedService,
│   │                               # IMenuService, IWireGuardService, ITailscaleService,
│   │                               # IAlertService, INotificationChannelService
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
│   ├── WireGuardSettings.cs        # Host, Port, Username, Password, Interface
│   ├── TailscaleSettings.cs        # ApiKey, Tailnet
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

### LDAP — `/api/ldap`
| Método | Ruta                        | Auth          | Descripción                                       |
|--------|-----------------------------|---------------|---------------------------------------------------|
| GET    | `/users/{gidNumber}`        | `[Authorize]` | Lista usuarios del grupo (primarios + memberUid)  |
| POST   | `/users`                    | `[Authorize]` | Crea usuario en LDAP con contraseña SSHA          |
| PUT    | `/users/{username}`         | `[Authorize]` | Actualiza FirstName, LastName y/o GidNumber       |
| DELETE | `/users/{username}/disable` | `[Authorize]` | Soft-delete: `description=DISABLED` + lock hash   |
| PUT    | `/users/{username}/enable`  | `[Authorize]` | Reactiva usuario: elimina atributo `description`  |

### Permission — `/api/permission`
| Método | Ruta     | Auth          | Descripción                               |
|--------|----------|---------------|-------------------------------------------|
| GET    | `/Menus` | `[Authorize]` | Menús accesibles según gidNumber del JWT  |

### Monitoring — `/api/monitoring`
| Método | Ruta             | Auth | Descripción                   |
|--------|------------------|------|-------------------------------|
| GET    | `/device-types`  | —    | Tipos de dispositivo LibreNMS |
| GET    | `/grafana-panels`| —    | Paneles Grafana desde DB      |

### Netbox — `/api/netbox`
CRUD completo para cada recurso. Todos los endpoints siguen el patrón `GET /`, `GET /{id}`, `POST /`, `PATCH /{id}`, `DELETE /{id}`.

| Recurso             | Ruta base                    | Auth |
|---------------------|------------------------------|------|
| Regiones            | `/regions`                   | —    |
| IPs                 | `/ip-addresses`              | —    |
| VLANs               | `/vlans`                     | —    |
| Cables              | `/cables`                    | —    |
| Sitios              | `/sites`                     | —    |
| Dispositivos        | `/devices`                   | —    |
| Racks               | `/racks`                     | —    |
| Fabricantes         | `/manufacturers`             | —    |
| Roles de dispositivo| `/device-roles`              | —    |
| Module Type Profiles| `/module-type-profiles`      | —    |

### Alertas — `/api/alerts`
| Método | Ruta        | Auth | Descripción                                          |
|--------|-------------|------|------------------------------------------------------|
| POST   | `/grafana`  | —    | Webhook Grafana → procesa alerta → notifica Telegram |
| POST   | `/librenms` | —    | Webhook LibreNMS → procesa alerta → notifica Telegram|

Ambos aceptan `?channelId=N` opcional para enviar a un canal específico; sin él, notifica a todos los canales activos.

### Canales de Notificación — `/api/notification-channels`
| Método | Ruta              | Auth | Descripción                                    |
|--------|-------------------|------|------------------------------------------------|
| GET    | `/`               | —    | Lista todos los canales                        |
| GET    | `/{id}`           | —    | Detalle de un canal                            |
| POST   | `/`               | —    | Crea canal (BotToken, ChatId, Name)            |
| PUT    | `/{id}`           | —    | Actualiza canal                                |
| DELETE | `/{id}`           | —    | Elimina canal                                  |
| POST   | `/{id}/test`      | —    | Envía mensaje de prueba al canal               |
| POST   | `/test-direct`    | —    | Envía prueba con BotToken y ChatId en el body  |

### Oxidized — `/api/oxidized`
| Método | Ruta                            | Auth          | Descripción                        |
|--------|---------------------------------|---------------|------------------------------------|
| GET    | `/devices`                      | `[Authorize]` | Lista dispositivos Oxidized        |
| GET    | `/devices/{deviceName}/versions`| `[Authorize]` | Historial de versiones             |
| GET    | `/devices/{deviceName}/backup`  | `[Authorize]` | Backup de configuración            |

### VPN — `/api/vpn`
| Método | Ruta                | Auth          | Descripción                                                      |
|--------|---------------------|---------------|------------------------------------------------------------------|
| GET    | `/status`           | `[Authorize]` | Estado del gateway: servidor, IP gestión, uptime                 |
| GET    | `/wireguard`        | `[Authorize]` | Interfaz wg0, red, peers con handshake, RX/TX, estado online     |
| GET    | `/wireguard/stats`  | `[Authorize]` | Métricas /proc/net/dev: bytes, paquetes, errores RX/TX + timestamp |
| GET    | `/tailscale`        | `[Authorize]` | Estado Tailscale, IP mesh, nodo local, lista de peers            |
| GET    | `/access-policy`    | `[Authorize]` | Política estática: redes alcanzables desde 172.16.40.0/24        |

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
| ILdapAuthService      | LdapAuthService      | Scoped         | Autenticación + CRUD de usuarios LDAP                   |
| IRadiusSessionService | RadiusSessionService | Scoped         | Accounting Start/Stop RADIUS                            |
| IGrafanaService       | GrafanaService       | Scoped         | Construye URLs de paneles Grafana desde DB              |
| ILibreNmsService      | LibreNmsService      | AddHttpClient  | Consulta tipos de dispositivo LibreNMS                  |
| INetboxService        | NetboxService        | AddHttpClient  | CRUD regiones e IPs en Netbox                           |
| IOxidizedService      | OxidizedService      | AddHttpClient  | Dispositivos, versiones y backups de Oxidized           |
| IMenuService                | MenuService                | Scoped         | Obtiene menús accesibles por gidNumbers del usuario     |
| IWireGuardService           | WireGuardService           | Scoped         | Estado WireGuard y métricas via SSH (SSH.NET)           |
| ITailscaleService           | TailscaleService           | AddHttpClient  | Peers Tailscale via REST API + política de acceso VPN   |
| IAlertService               | AlertService               | Scoped         | Procesa webhooks de Grafana/LibreNMS → Telegram         |
| INotificationChannelService | NotificationChannelService | Scoped         | CRUD canales Telegram + envío de mensajes de prueba     |

### Métodos clave de ILdapAuthService
```csharp
LdapUser? Authenticate(string username, string password)
List<LdapRole> GetRoles()                          // Busca grupos LDAP (cn, gidNumber, description)
List<LdapUser> GetUsersByGid(string gidNumber)     // Primarios (gidNumber en usuario) + suplementarios (memberUid en grupo)
                                                   // LdapUser.IsActive = false cuando description == "DISABLED"
void CreateUser(CreateLdapUserDto dto)             // Crea usuario con hash SSHA, uidNumber auto-generado
void UpdateUser(string username, UpdateLdapUserDto dto) // FirstName, LastName, GidNumber (null = no cambiar)
void DisableUser(string username)                  // Soft-delete: description=DISABLED + prefijo "!" en hash
void EnableUser(string username)                   // Elimina atributo description (DirectoryAttributeOperation.Delete)
```

### Métodos clave de IWireGuardService
```csharp
Task<VpnGatewayStatusDto> GetGatewayStatusAsync() // SSH → cat /proc/uptime → uptime formateado
Task<WireGuardStatusDto>  GetStatusAsync()         // SSH → sudo wg show + sudo cat wg0.conf → peers parseados
Task<WireGuardStatsDto>   GetStatsAsync()          // SSH → cat /proc/net/dev → bytes/paquetes/errores RX/TX
```

### Métodos clave de ITailscaleService
```csharp
Task<TailscaleStatusDto> GetStatusAsync()  // GET https://api.tailscale.com/api/v2/tailnet/{tailnet}/devices
VpnAccessPolicyDto       GetAccessPolicy() // Política estática hardcodeada (VLAN50 → redes internas + Internet)
```

### Comportamiento del campo `online` de Tailscale
El campo `online` de la REST API de Tailscale es intermitente — unas llamadas lo incluyen, otras no. La implementación usa doble condición con OR:
```csharp
var isOnlineApi    = device.TryGetProperty("online", out var p) && p.GetBoolean();
var isRecentlySeen = lastSeenRaw != null
    && DateTimeOffset.TryParse(lastSeenRaw, out var dt)
    && (DateTimeOffset.UtcNow - dt).TotalSeconds <= 10;
var isOnline = isOnlineApi || isRecentlySeen;
```
El threshold de `lastSeen` es 10 segundos — suficientemente bajo para no generar falsos positivos, y sirve de seguro cuando `online` no llega en el JSON.

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

**LdapUser:** DisplayName, FirstName, LastName, Username, UidNumber, GidNumber, Department, Title, Groups, Roles, IsActive

**LdapRole:** Id (gidNumber), Name (cn), Description

## DTOs

| Carpeta       | DTOs                                                                                              |
|---------------|---------------------------------------------------------------------------------------------------|
| Auth/         | LoginRequestDto, LoginResponseDto, LdapUserDto, UserSessionDto, LdapRoleDto, CreateLdapUserDto, UpdateLdapUserDto |
| Alert/        | GrafanaWebhookDto, LibreNmsWebhookDto                                                             |
| Menu/         | MenuBlockDto `{ Block, Order, Menus }`, MenuItemDto `{ Id, Name, Slug, IsAssigned }`, AssignMenusRequestDto |
| Monitoring/   | DeviceTypeDto, GrafanaPanelDto                                                                    |
| Netbox/       | NetboxRegionDto, NetboxIpAddressDto, NetboxVlanDto, NetboxCableDto, NetboxSiteDto, NetboxDeviceDto, NetboxRackDto, NetboxManufacturerDto, NetboxDeviceRoleDto, NetboxModuleTypeProfileDto + Create*/Update* por cada recurso |
| Notification/ | NotificationChannelDto, CreateNotificationChannelDto, UpdateNotificationChannelDto, TestNotificationDirectDto |
| Oxidized/     | OxidizedDeviceDto, OxidizedBackupDto, OxidizedVersionDto                                         |
| Vpn/          | VpnGatewayStatusDto, WireGuardStatusDto, WireGuardPeerDto, WireGuardStatsDto, TailscaleStatusDto, TailscalePeerDto, VpnAccessPolicyDto |

## Integaciones Externas

| Sistema    | IP/URL                                      | Auth             | Config key en appsettings |
|------------|---------------------------------------------|------------------|---------------------------|
| LDAP       | 172.16.20.17:389                            | Bind user/pass   | `Ldap:`                   |
| RADIUS     | 172.16.20.12:1813                           | Shared Secret    | `Radius:`                 |
| Oxidized   | 172.16.20.13                                | Basic Auth       | `Oxidized:`               |
| LibreNMS   | 172.16.20.10                                | X-Auth-Token     | `LibreNMS:`               |
| Grafana    | 172.16.20.10:3000                           | Solo URL         | `Grafana:`                |
| Netbox     | 172.16.20.11                                | Bearer Token     | `Netbox:`                 |
| PostgreSQL | 172.16.20.15                                | User/Pass        | `ConnectionStrings:`      |
| WireGuard  | 172.16.20.12 (SSH :22)                      | SSH user/pass    | `WireGuard:`              |
| Tailscale  | api.tailscale.com/api/v2/tailnet/{tailnet}  | Bearer ApiKey    | `Tailscale:`              |

## Settings (IOptions<T>)

| Clase              | Sección appsettings | Campos clave                                  |
|--------------------|---------------------|-----------------------------------------------|
| JwtSettings        | `Jwt:`              |                                               |
| LdapSettings       | `Ldap:`             | Host, Port, BaseDn, ServiceUser, ServicePass, DefaultPassword |
| RadiusSettings     | `Radius:`           |                                               |
| OxidizedSettings   | `Oxidized:`         |                                               |
| WireGuardSettings  | `WireGuard:`        | Host (172.16.20.12), Port (22), Username, Password, Interface (wg0) |
| TailscaleSettings  | `Tailscale:`        | ApiKey (tskey-api-...), Tailnet (email o dominio) |

## Gestión de Usuarios LDAP

### Creación de usuario
- `uidNumber` se auto-genera tomando el máximo existente en la OU + 1
- Contraseña hasheada con **SSHA** (SHA1 + salt aleatorio de 4 bytes), almacenada como `{SSHA}base64`
- Si no se provee contraseña en el request, se usa `LdapSettings.DefaultPassword`
- `objectClass`: inetOrgPerson, posixAccount, shadowAccount

### Soft-delete (DisableUser)
- Añade atributo `description = "DISABLED"` al usuario
- Prefija `"!"` al hash de contraseña para bloquear el bind
- `LdapUser.IsActive` es `false` cuando `description == "DISABLED"`

### Habilitar usuario (EnableUser)
- Usa `DirectoryAttributeOperation.Delete` sobre el atributo `description` (sin valor)
- **No usar Replace con string vacío** — OpenLDAP rechaza esa operación con `InvalidAttributeSyntax`

### Patrón para modificar atributos LDAP
```csharp
// Usar helper Mod() para evitar error "Declarador de miembro de inicializador no válido"
private static DirectoryAttributeModification Mod(string name, DirectoryAttributeOperation op, string? value = null)
{
    var mod = new DirectoryAttributeModification { Name = name, Operation = op };
    if (value != null) mod.Add(value);
    return mod;
}
```

## Módulo VPN

### Arquitectura VPN
- **WireGuardService** (Scoped, SSH.NET): conecta a `172.16.20.12` via SSH y ejecuta:
  - `sudo wg show wg0` → estado de la interfaz y peers
  - `sudo cat /etc/wireguard/wg0.conf` → red VPN, IP servidor, nombres de peers
  - `cat /proc/net/dev` → métricas RX/TX sin sudo
  - `cat /proc/uptime` → uptime del gateway
- **TailscaleService** (AddHttpClient): llama a `api.tailscale.com/api/v2/tailnet/{tailnet}/devices`

### sudo via SSH sin TTY
SSH.NET no asigna pseudo-terminal. Usar `sudo -S` con stdin para pasar la contraseña:
```csharp
private string RunSudoCommand(SshClient client, string command)
{
    using var cmd = client.CreateCommand($"sudo -S {command} 2>/dev/null");
    var asyncResult = cmd.BeginExecute();
    using var stdin = new StreamWriter(cmd.CreateInputStream());
    stdin.WriteLine(_settings.Password);
    stdin.Flush();
    cmd.EndExecute(asyncResult);
    return cmd.Result.Trim();
}
```

### Lógica de estado WireGuard
- **Peer online**: último handshake ≤ 180 segundos (WireGuard renegocia cada ~2 min)
- **Nombre del peer**: busca comentario `# Name` en wg0.conf sobre la línea `PublicKey`; si no existe, usa `"Cliente-{lastOctet}"` de la IP VPN
- **RX/TX**: parseados de `"13.67 MiB received, 29.48 MiB sent"` → convertidos a bytes
- **Red VPN**: extraída del campo `Address` en `[Interface]` de wg0.conf

### Política de acceso VPN (estática)
Origen: `172.16.40.0/24` → acceso permitido a: `172.16.20.0/24`, `172.16.30.0/24`, `172.16.50.0/24`, `172.16.80.0/24`, Internet

### Uso de /wireguard/stats para gráfico de bandwidth
El endpoint retorna bytes acumulativos + timestamp UTC. El frontend calcula:
`Mbps = (ΔBytes × 8) / (ΔSeconds × 1_000_000)` comparando dos snapshots consecutivos.

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
- XML `<summary>` en todos los métodos de interfaces y DTOs

## Estado Actual del Proyecto

- **Listo:** Autenticación LDAP → JWT completa con RBAC por gidNumber
- **Listo:** Registro de sesiones web en tabla `useractivities`
- **Listo:** Paneles Grafana dinámicos desde tabla `grafanapanels`
- **Listo:** Integración Oxidized — dispositivos, versiones, backups
- **Listo:** Integración Netbox — CRUD completo: regiones, IPs, VLANs, cables, sitios, dispositivos, racks, fabricantes, roles, module-type-profiles (NetboxController `/api/netbox`)
- **Listo:** Integración LibreNMS — tipos de dispositivo
- **Listo:** LDAP GetRoles() y GetUsersByGid() con campo IsActive
- **Listo:** LDAP CreateUser, UpdateUser, DisableUser (soft-delete), EnableUser
- **Listo:** Entidades EF Core para sistema de menús (MenuBlock, Menu, RoleMenu — schema `session`)
- **Listo:** MenuRepository con filtro por gidNumbers + menús públicos
- **Listo:** MenuService.GetMenusForUser() — mapea a MenuBlockDto agrupado
- **Listo:** VPN Dashboard — WireGuard (SSH), Tailscale (REST API), Access Policy, Bandwidth Stats
- **Listo:** Sistema de alertas — webhooks Grafana y LibreNMS → Telegram (AlertController + AlertService)
- **Listo:** CRUD canales de notificación Telegram con test directo (NotificationChannelController)
- **En progreso:** PermissionController — GET /api/permission/Menus activo, faltan endpoints de gestión (asignar/desasignar)
- **Suspendido:** RADIUS Accounting-Start en login — RADIUS es para equipos de red, no sesiones web
- **Pendiente:** Restaurar `[Authorize]` en `MonitoringController`, `NetboxController`, `AlertController`, `NotificationChannelController` y `VpnController` (sin auth actualmente)
- **Pendiente:** Endpoints admin de gestión de roles-menús (AssignMenuToRole, RemoveMenuFromRole)

## Puertos Locales

| Perfil       | HTTP            | HTTPS           |
|--------------|-----------------|-----------------|
| Dev (dotnet) | localhost:5182  | localhost:7073  |
| IIS Express  | localhost:31819 | localhost:44356 |

Swagger UI disponible en `/swagger` al correr en Development.
