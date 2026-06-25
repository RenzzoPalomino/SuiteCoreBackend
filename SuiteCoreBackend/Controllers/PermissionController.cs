using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Enums;
using SuiteCoreBackend.Services.Implementations;
using SuiteCoreBackend.Services.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuiteCoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        //private readonly ILdapAuthService _ldapService;
        private readonly IMenuService _menuService;

        public PermissionController(IMenuService menuService)
        {
            _menuService  = menuService;
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

        //TODO: Implementar endpoint para asignar y desasignar menus a roles, con validación de permisos de administrador.
    }
}
