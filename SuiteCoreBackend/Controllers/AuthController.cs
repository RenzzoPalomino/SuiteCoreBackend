using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Services.Interfaces;
using System.Security.Claims;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Inicializa el controlador de autenticación.
    /// </summary>
    /// <param name="authService">Servicio de autenticación.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Valida las credenciales del usuario y devuelve un token JWT.
    /// </summary>
    /// <param name="request">Credenciales de acceso.</param>
    /// <returns>Datos de sesión del usuario autenticado.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (request is null)
        {
            return BadRequest(new
            {
                message = "La solicitud no es válida."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new
            {
                message = "El correo y la contraseña son obligatorios."
            });
        }

        var response = _authService.Login(request);

        if (response is null)
        {
            return Unauthorized(new
            {
                message = "Credenciales incorrectas o usuario inactivo."
            });
        }

        return Ok(response);
    }

    /// <summary>
    /// Obtiene los datos principales del usuario autenticado desde el token JWT.
    /// </summary>
    /// <returns>Información básica del usuario autenticado.</returns>
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Id = userId,
            Email = email,
            Rol = role
        });
    }

    /// <summary>
    /// Valida el acceso a un endpoint exclusivo para administradores.
    /// </summary>
    /// <returns>Mensaje de confirmación de acceso administrativo.</returns>
    [HttpGet("admin")]
    [Authorize(Roles = "Administrador")]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            message = "Acceso permitido solo para Administradores."
        });
    }
}