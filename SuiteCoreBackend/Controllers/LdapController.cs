using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Enums;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/ldap")]
[Authorize]
public class LdapController : ControllerBase
{
    private readonly ILdapAuthService _ldapService;

    public LdapController(ILdapAuthService ldapService)
    {
        _ldapService = ldapService;
    }

    [HttpPost("users")]
    [Authorize]
    public IActionResult CreateUser([FromBody] CreateLdapUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.GidNumber))
            return BadRequest(new { message = "Username, password, nombre, apellido y rol son obligatorios." });

        try
        {
            var user = _ldapService.CreateUser(dto);
            return CreatedAtAction(nameof(CreateUser), new { username = user.Username }, user);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("users/{username}")]
    [Authorize]
    public IActionResult UpdateUser(string username, [FromBody] UpdateLdapUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            return BadRequest(new { message = "Nombre y apellido son obligatorios." });

        try
        {
            var user = _ldapService.UpdateUser(username, dto);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPatch("users/{username}/enable")]
    [Authorize]
    public IActionResult EnableUser(string username)
    {
        try
        {
            _ldapService.EnableUser(username);
            return Ok(new { message = $"Usuario '{username}' habilitado correctamente. Se asignó la contraseña por defecto." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("users/{username}")]
    [Authorize]
    public IActionResult DisableUser(string username)
    {
        try
        {
            _ldapService.DisableUser(username);
            return Ok(new { message = $"Usuario '{username}' deshabilitado correctamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
