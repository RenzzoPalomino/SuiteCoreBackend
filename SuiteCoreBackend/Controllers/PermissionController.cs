using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.DTOs.Menu;
using SuiteCoreBackend.Services.Interfaces;
using System.Security.Claims;

namespace SuiteCoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly ILdapAuthService _ldapService;

        public PermissionController(IMenuService menuService, ILdapAuthService ldapService)
        {
            _menuService = menuService;
            _ldapService = ldapService;
        }

        [HttpGet("Menus")]
        [Authorize]
        public async Task<ActionResult> MenusByRole()
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                //var role = RoleGroupCodes.NOC; //testing
                List<string> roles = new List<string>{role};

                var menus = await _menuService.GetMenusForUser(roles);

                if (menus is null || menus.Count == 0)
                    return NotFound(new { message = "No se encontraron menus para el rol especificado." });

                return Ok(new
                {
                    role,
                    total = menus.Count,
                    menus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("Roles")]
        [Authorize]
        public ActionResult GetRolesWithUsers()
        {
            try
            {
                var roles = _ldapService.GetRoles();

                var result = roles.Select(role =>
                {
                    var users = _ldapService.GetUsersByGid(role.Id)
                        .Select(u => new LdapUserDto
                        {
                            DisplayName = u.DisplayName,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Username = u.Username
                        }).ToList();

                    return new LdapRoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        TotalUsers = users.Count,
                        Users = users
                    };
                }).ToList();

                return Ok(new
                {
                    total = result.Count,
                    roles = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("Roles/{gidNumber}/menus")]
        [Authorize]
        public async Task<ActionResult> GetMenusByRole(string gidNumber)
        {
            try
            {
                var menus = await _menuService.GetMenusByRole(gidNumber);

                if (menus is null || menus.Count == 0)
                    return NotFound(new { message = "No se encontraron menús asignados a este rol." });

                return Ok(new
                {
                    gidNumber,
                    total = menus.Count,
                    menus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("Roles/{gidNumber}/menus")]
        [Authorize]
        public async Task<ActionResult> AssignMenusToRole(string gidNumber, [FromBody] AssignMenusRequestDto request)
        {
            try
            {
                var modifiedBy = User.FindFirst("username")?.Value;
                await _menuService.AssignMenusToRole(gidNumber, request.MenuIds, modifiedBy);
                return Ok(new { message = "Menús asignados correctamente.", gidNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
