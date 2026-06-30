namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Representación de un grupo/rol del directorio LDAP.
    /// Agrupa el gidNumber, el nombre del grupo (cn) y los usuarios que pertenecen a él.
    /// </summary>
    public class LdapRoleDto
    {
        /// <summary>Identificador numérico del grupo (atributo gidNumber en LDAP). Usado como clave RBAC.</summary>
        public string Id { get; set; } = "";

        /// <summary>Nombre del grupo tal como está definido en LDAP (atributo cn). Ej: "NOC", "Network Admin".</summary>
        public string Name { get; set; } = "";

        /// <summary>Descripción del rol definida en el directorio LDAP.</summary>
        public string Description { get; set; } = "";

        /// <summary>Total de usuarios que pertenecen a este rol (primarios + suplementarios).</summary>
        public int TotalUsers { get; set; }

        /// <summary>Lista de usuarios miembros del rol, tanto primarios (gidNumber) como suplementarios (memberUid).</summary>
        public List<LdapUserDto> Users { get; set; } = new();
    }
}
