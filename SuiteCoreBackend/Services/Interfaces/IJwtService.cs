using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT para un usuario autenticado.
        /// </summary>
        /// <param name="userId">Usuario LDAP.</param> 
        /// <returns>Token JWT firmado.</returns>
        string GenerateToken(LdapUser user);
    }
}
