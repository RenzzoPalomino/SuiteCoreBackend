using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Cuerpo de la petición POST /api/auth/login.
    /// Contiene las credenciales del usuario para autenticación contra el directorio LDAP.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>Nombre de usuario LDAP (atributo uid). Ej: "rpalomino".</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>Contraseña del usuario. Se valida mediante bind LDAP con las credenciales proporcionadas.</summary>
        public string Password { get; set; } = string.Empty;
    }
}
