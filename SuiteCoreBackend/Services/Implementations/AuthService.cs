using AutoMapper;
using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;

namespace SuiteCoreBackend.Services.Implementations;

/// <summary>
/// Servicio encargado de validar credenciales y generar sesiones JWT.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ITestUserService _testUserService;
    private readonly ILdapAuthService   _ldapAuthService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa el servicio de autenticación.
    /// </summary>
    /// <param name="testUserService">Servicio temporal de usuarios de prueba.</param>
    /// <param name="jwtService">Servicio generador de tokens JWT.</param>
    /// <param name="jwtOptions">Configuración JWT de la aplicación.</param>
    public AuthService(
        ITestUserService testUserService,
        ILdapAuthService ldapAuthService,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtOptions
        ,IMapper mapper)
    {
        _testUserService = testUserService;
        _jwtService = jwtService;
        _jwtSettings = jwtOptions.Value;
        _ldapAuthService = ldapAuthService;
        _mapper = mapper;
    }

    /// <summary>
    /// Valida las credenciales del usuario y devuelve una sesión JWT si son correctas.
    /// </summary>
    /// <param name="request">Credenciales de acceso.</param>
    /// <returns>Datos de sesión del usuario autenticado.</returns>
    public LoginResponseDto? Login(LoginRequestDto request)
    {
        LdapUser user = _ldapAuthService.Authenticate(request.Username, request.Password);

        if (user is null)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(user);

        LdapUserDto userDto = _mapper.Map<LdapUserDto>(user);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            User = userDto
        };
    }
}