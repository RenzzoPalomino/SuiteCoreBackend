using Microsoft.AspNetCore.Authentication.JwtBearer;
 using Microsoft.IdentityModel.Tokens;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Implementations;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Services.Monitoring;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Infrastructure.Implementations;
using SuiteCoreBackend.Settings; 
using System.Text;
using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Infrastructure.Context;

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
// ---------------------------------------------------------
// 2. Leemos la sección "Jwt" desde appsettings.json
// ---------------------------------------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

builder.Services.Configure<LdapSettings>(
    builder.Configuration.GetSection("Ldap"));



builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITestUserService, TestUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILdapAuthService, LdapAuthService>(); /**/
builder.Services.AddHttpClient<ILibreNmsService, LibreNmsService>();
builder.Services.AddScoped<IGrafanaService, GrafanaService>();
builder.Services.AddScoped<IGrafanaRepository, GrafanaRepository>();

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

            // Valida que el token esté dirigido a nuestra audiencia esperada
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            // Valida que el token no esté vencido
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
// 8. Redireccionamiento HTTPS
// ---------------------------------------------------------
app.UseHttpsRedirection();
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
