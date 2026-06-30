namespace SuiteCoreBackend.DTOs.Auth
{
    /// <summary>
    /// Datos de sesión del usuario actualmente autenticado.
    /// Retornado por GET /api/auth/me a partir de los claims del JWT.
    /// </summary>
    public class UserSessionDto
    {
        /// <summary>Identificador único de la sesión activa.</summary>
        public Guid Id { get; set; }

        /// <summary>Nombre completo del usuario autenticado.</summary>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>Correo electrónico del usuario (si está disponible en los claims).</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Rol activo del usuario en la sesión, derivado del claim gidNumber.</summary>
        public string Rol { get; set; } = string.Empty;
    }
}
