namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Cuerpo de la petición POST /api/ldap/users para crear un nuevo usuario en LDAP.
    /// Los campos username, password, nombre, apellido y gidNumber son obligatorios.
    /// El uidNumber se genera automáticamente (máximo existente + 1).
    /// </summary>
    public class CreateLdapUserDto
    {
        /// <summary>Primer nombre del usuario. Se usa para givenName y componer displayName y cn.</summary>
        public string FirstName { get; set; } = "";

        /// <summary>Apellido del usuario. Se usa para sn y componer displayName y cn.</summary>
        public string LastName { get; set; } = "";

        /// <summary>Nombre de usuario único (atributo uid). Define el DN: uid={Username},ou=People,{BaseDn}.</summary>
        public string Username { get; set; } = "";

        /// <summary>Contraseña inicial del usuario. Se almacena en LDAP con hash SSHA (SHA1 + salt).</summary>
        public string Password { get; set; } = "";

        /// <summary>Número de grupo primario (gidNumber) que define el rol RBAC del usuario.</summary>
        public string GidNumber { get; set; } = "";

        /// <summary>Departamento del usuario (atributo departmentNumber). Opcional.</summary>
        public string Department { get; set; } = "";

        /// <summary>Cargo del usuario dentro de la organización (atributo title). Opcional.</summary>
        public string Title { get; set; } = "";
    }
}
