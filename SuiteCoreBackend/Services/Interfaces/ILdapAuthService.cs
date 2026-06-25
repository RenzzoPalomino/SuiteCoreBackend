using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ILdapAuthService
    {
        /// <summary>
        /// Metodo para autenticar un usuario en LDAP
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        LdapUser? Authenticate(string username, string password);
        /// <summary>
        /// Método para obtener todos los roles de LDAP
        /// </summary>
        /// <returns></returns>
        List<LdapRole> GetRoles();
        /// <summary>
        /// Método para obtener todos los usuarios de LDAP
        /// </summary>
        /// <param name="gidNumber"></param>
        /// <returns></returns>
        List<LdapUser> GetUsersByGid(string gidNumber);
    }
}
