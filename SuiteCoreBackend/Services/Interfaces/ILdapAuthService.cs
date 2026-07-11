using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ILdapAuthService
    {
        /// <summary>
        /// Autentica un usuario contra el directorio LDAP mediante bind con sus credenciales.
        /// Retorna null si el usuario no existe; lanza excepción si la contraseña es incorrecta.
        /// </summary>
        LdapUser? Authenticate(string username, string password);

        /// <summary>
        /// Obtiene todos los grupos (roles) definidos en el directorio LDAP.
        /// </summary>
        List<LdapRole> GetRoles();

        /// <summary>
        /// Obtiene los usuarios que pertenecen a un gidNumber dado,
        /// incluyendo miembros primarios (gidNumber en el usuario) y suplementarios (memberUid en el grupo).
        /// </summary>
        List<LdapUser> GetUsersByGid(string gidNumber);

        /// <summary>
        /// Crea un nuevo usuario en LDAP bajo ou=People con los objectClass inetOrgPerson y posixAccount.
        /// El uidNumber se genera automáticamente tomando el máximo existente + 1.
        /// </summary>
        LdapUser CreateUser(CreateLdapUserDto dto);

        /// <summary>
        /// Actualiza el nombre, apellido y opcionalmente el rol (gidNumber) de un usuario existente en LDAP.
        /// </summary>
        LdapUser UpdateUser(string username, UpdateLdapUserDto dto);

        /// <summary>
        /// Deshabilita un usuario sin eliminarlo del directorio LDAP (soft delete).
        /// Invalida la contraseña con un hash bloqueado e indica el estado con description=DISABLED.
        /// </summary>
        void DisableUser(string username);

        /// <summary>
        /// Rehabilita un usuario previamente deshabilitado, asignándole la contraseña por defecto
        /// configurada en LdapSettings:DefaultPassword.
        /// </summary>
        void EnableUser(string username);
    }
}
