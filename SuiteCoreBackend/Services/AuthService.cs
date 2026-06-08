using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;

namespace SuiteCoreBackend.Services;

/// <summary>
/// Servicio encargado de validar credenciales y generar sesiones JWT.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ITestUserService _testUserService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Inicializa el servicio de autenticación.
    /// </summary>
    /// <param name="testUserService">Servicio temporal de usuarios de prueba.</param>
    /// <param name="jwtService">Servicio generador de tokens JWT.</param>
    /// <param name="jwtOptions">Configuración JWT de la aplicación.</param>
    public AuthService(
        ITestUserService testUserService,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtOptions)
    {
        _testUserService = testUserService;
        _jwtService = jwtService;
        _jwtSettings = jwtOptions.Value;
    }

    /// <summary>
    /// Valida las credenciales del usuario y devuelve una sesión JWT si son correctas.
    /// </summary>
    /// <param name="request">Credenciales de acceso.</param>
    /// <returns>Datos de sesión del usuario autenticado.</returns>
    public LoginResponseDto? Login(LoginRequestDto request)
    {
        var user = _testUserService.ValidateCredentials(
            request.Email,
            request.Password
        );

        if (user is null || !user.Activo)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.Rol
        );

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            User = new UserSessionDto
            {
                Id = user.Id,
                NombreCompleto = user.NombreCompleto,
                Email = user.Email,
                Rol = user.Rol
            }
        };
    }
}