namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT para un usuario autenticado.
        /// </summary>
        /// <param name="userId">Identificador único del usuario.</param>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="role">Rol asignado al usuario.</param>
        /// <returns>Token JWT firmado.</returns>
        string GenerateToken(Guid userId, string email, string role);
    }
}
