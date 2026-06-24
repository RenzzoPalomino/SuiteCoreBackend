using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Enums;
using SuiteCoreBackend.Services.Interfaces;
using System.Security.Claims;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    //private readonly ISessionPropertyService _sessionPropertyService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (request is null)
                return BadRequest(new { message = "La solicitud no es válida." });

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "El usuario y la contraseña son obligatorios." });

            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var response = await _authService.LoginAsync(request, clientIp);

            if (response is null)
                return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var sessionId = User.FindFirst("sessionId")?.Value;
        var username = User.FindFirst("username")?.Value;

        if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(username))
            await _authService.LogoutAsync(sessionId, username);

        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst("username")?.Value;
        var name = User.FindFirst(ClaimTypes.GivenName)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var sessionId = User.FindFirst("sessionId")?.Value;
        var department = User.FindFirst("department")?.Value;

        return Ok(new
        {
            Id = userId,
            Username = username,
            Name = name,
            Rol = role,
            SessionId = sessionId,
            Department = department
        });
    }

    [HttpGet("admin")]
    [Authorize(Roles = $"{RoleGroupCodes.NETWORK_ADMIN},{RoleGroupCodes.IT_SUPERVISOR}")]
    public IActionResult AdminOnly()
    {
        return Ok(new { message = "Acceso permitido solo para Administradores." });
    }

    //[HttpGet("Access")]
    //[Authorize]
    //public ActionResult MenusByRole()
    //{
    //    var role = User.FindFirst(ClaimTypes.Role)?.Value;

    //    var menus = _authService.GetMenusByRole(role);

    //}
}
