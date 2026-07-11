namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Representación de un usuario LDAP para exposición en la API.
    /// Mapeado desde <c>LdapUser</c> via AutoMapper. Incluye datos del perfil,
    /// rol (gidNumber) y estado de actividad.
    /// </summary>
    public class LdapUserDto
    {
        /// <summary>Nombre completo del usuario (atributo displayName en LDAP).</summary>
        public string DisplayName { get; set; } = "";

        /// <summary>Primer nombre del usuario (atributo givenName en LDAP).</summary>
        public string FirstName { get; set; } = "";

        /// <summary>Apellido del usuario (atributo sn en LDAP).</summary>
        public string LastName { get; set; } = "";

        /// <summary>Nombre de usuario único en LDAP (atributo uid). Usado como identificador en el sistema.</summary>
        public string Username { get; set; } = "";

        /// <summary>Identificador numérico único del usuario en el sistema POSIX (atributo uidNumber).</summary>
        public string UidNumber { get; set; } = "";

        /// <summary>Número de grupo primario del usuario (atributo gidNumber). Define el rol RBAC en el sistema.</summary>
        public string GidNumber { get; set; } = "";

        /// <summary>Departamento al que pertenece el usuario (atributo departmentNumber en LDAP).</summary>
        public string Department { get; set; } = "";

        /// <summary>Cargo o título del usuario dentro de la organización (atributo title en LDAP).</summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Indica si el usuario está habilitado. False cuando el atributo description contiene "DISABLED",
        /// lo que implica que su contraseña fue invalidada mediante soft-delete.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Lista de DNs de grupos LDAP a los que pertenece el usuario (atributo memberOf).</summary>
        public List<string> Groups { get; set; } = new();

        /// <summary>Lista de nombres de roles extraídos del DN de cada grupo (cn del grupo).</summary>
        public List<string> Roles { get; set; } = new();
    }
}
