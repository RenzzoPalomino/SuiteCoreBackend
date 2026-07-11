namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Respuesta del endpoint POST /api/auth/login cuando la autenticación es exitosa.
    /// Contiene el token JWT, su fecha de expiración, los datos del usuario y el identificador de sesión.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>Token JWT firmado con HS256. Debe enviarse en el header Authorization: Bearer {Token}.</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>Fecha y hora exacta en que expira el token (UTC). Configurada por JwtSettings:ExpiresInMinutes.</summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>Datos del usuario autenticado mapeados desde LDAP.</summary>
        public LdapUserDto User { get; set; } = new();

        /// <summary>Identificador único de la sesión (GUID de 16 chars). Registrado en la tabla useractivities.</summary>
        public string SessionId { get; set; } = string.Empty;
    }
}
