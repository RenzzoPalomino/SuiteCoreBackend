namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Cuerpo de la petición PUT /api/ldap/users/{username} para actualizar un usuario LDAP.
    /// Solo permite modificar nombre, apellido y opcionalmente el rol (gidNumber).
    /// FirstName y LastName son obligatorios; GidNumber es opcional.
    /// </summary>
    public class UpdateLdapUserDto
    {
        /// <summary>Nuevo primer nombre del usuario. Actualiza givenName, cn y displayName en LDAP.</summary>
        public string FirstName { get; set; } = "";

        /// <summary>Nuevo apellido del usuario. Actualiza sn, cn y displayName en LDAP.</summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// Nuevo gidNumber del usuario para cambiar su rol RBAC.
        /// Si es null o vacío, el gidNumber actual no se modifica.
        /// </summary>
        public string? GidNumber { get; set; }
    }
}
