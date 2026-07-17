using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Implementations;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Services.Implementations;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Services.Monitoring;
using SuiteCoreBackend.Settings; 
using System.Text;
//using SuiteCoreBackend.Infraestucture.Context;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. Agregamos los servicios base de la API
// ---------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

builder.Services.AddDbContext<SCDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", builder =>
    {
        builder.WithOrigins(allowedOrigins ?? Array.Empty<string>())
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ---------------------------------------------------------
// 2. Leemos la sección "Jwt" desde appsettings.json
// ---------------------------------------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

builder.Services.Configure<LdapSettings>(
    builder.Configuration.GetSection("Ldap"));

//builder.Services.Configure<RadiusSettings>(
//    builder.Configuration.GetSection("Radius"));

builder.Services.Configure<OxidizedSettings>(
    builder.Configuration.GetSection("Oxidized"));
builder.Services.AddHttpClient<IOxidizedService, OxidizedService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILdapAuthService, LdapAuthService>();
builder.Services.AddHttpClient<ILibreNmsService, LibreNmsService>();
builder.Services.AddHttpClient<INetboxService, NetboxService>();
builder.Services.AddScoped<IGrafanaService, GrafanaService>();
builder.Services.AddScoped<IGrafanaRepository, GrafanaRepository>();
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<INotificationChannelRepository, NotificationChannelRepository>();
builder.Services.AddHttpClient<INotificationChannelService, NotificationChannelService>();
builder.Services.AddScoped<IAlertEventRepository, AlertEventRepository>();
builder.Services.AddHttpClient<IAlertService, AlertService>();

builder.Services.Configure<WireGuardSettings>(
    builder.Configuration.GetSection("WireGuard"));
builder.Services.Configure<TailscaleSettings>(
    builder.Configuration.GetSection("Tailscale"));
builder.Services.AddScoped<IWireGuardService, WireGuardService>();
builder.Services.AddHttpClient<ITailscaleService, TailscaleService>();

builder.Services.Configure<ScnoSettings>(
    builder.Configuration.GetSection("Scno"));
builder.Services.AddHttpClient<ISdnService, SdnService>();
builder.Services.AddHttpClient<IGrafanaEmbedService, GrafanaEmbedService>();
builder.Services.AddHttpClient<IDashboardService, DashboardService>();
builder.Services.AddHttpClient<IInfrastructureService, InfrastructureService>();
builder.Services.AddHttpClient<INetworkService, NetworkService>();
builder.Services.AddHttpClient<ISdnSupervisionService, SdnSupervisionService>();
builder.Services.AddHttpClient<IIncidentsService, IncidentsService>();
builder.Services.AddHttpClient<IVpnSupervisionService, VpnSupervisionService>();
builder.Services.AddHttpClient<IReportsAuditService, ReportsAuditService>();




// 4. Convertimos la sección Jwt a un objeto JwtSettings
// para usar sus valores directamente en Program.cs
// ---------------------------------------------------------

var jwtSettings = jwtSection.Get<JwtSettings>();

if (jwtSettings is null)
{
    throw new Exception("La configuración JWT no fue encontrada en appsettings.json.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new Exception("La clave JWT no puede estar vacía.");
}
// ---------------------------------------------------------
// 5. Configuramos la autenticación con JWT Bearer
// Aquí indicamos cómo la API debe validar los tokens recibidos
// ---------------------------------------------------------

builder.Services
    .AddAuthentication(options =>
    {
        // Esquema por defecto para autenticar usuarios
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        // Esquema por defecto para retar solicitudes no autenticadas
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Parámetros de validación del token JWT
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Valida que el token haya sido emitido por nuestro issuer
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            // Valida que el token está dirigido a nuestra audiencia esperada
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            // Valida que el token no está vencido
            ValidateLifetime = true,

            // Valida la clave con la que fue firmado el token
            ValidateIssuerSigningKey = true,

            // Clave secreta usada para validar la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)
            ),

            // Elimina margen extra de expiración
            // Por defecto .NET da unos minutos adicionales
            ClockSkew = TimeSpan.Zero
        };
    });
// ---------------------------------------------------------
// 6. Agregamos autorización
// Esto permite usar [Authorize] en controllers y endpoints
// ---------------------------------------------------------
builder.Services.AddAuthorization();

var app = builder.Build();
// ---------------------------------------------------------
// 7. Configuramos Swagger solo en ambiente de desarrollo
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// ---------------------------------------------------------
// 8. Redireccionamiento HTTPS y habilitación de cors
// ---------------------------------------------------------
app.UseHttpsRedirection();
app.UseCors("AllowedOrigins");
// ---------------------------------------------------------
// 9. Activamos autenticación
// IMPORTANTE: debe ir antes de UseAuthorization()
// ---------------------------------------------------------
app.UseAuthentication();
// ---------------------------------------------------------
// 10. Activamos autorización
// Esto valida permisos, roles y endpoints protegidos
// ---------------------------------------------------------
app.UseAuthorization();
// ---------------------------------------------------------
// 11. Mapeamos los controladores
// ---------------------------------------------------------
app.MapControllers();
// ---------------------------------------------------------
// 12. Ejecutamos la aplicación
// ---------------------------------------------------------
app.Run();
