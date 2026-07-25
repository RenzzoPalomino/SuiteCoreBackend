using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService, ILdapAuthService ldapService)
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
}
