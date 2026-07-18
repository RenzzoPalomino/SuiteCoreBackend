using SuiteCoreBackend.DTOs.Auth;

namespace SuiteCoreBackend.Services.Interfaces
{
    /// <summary>
    /// Define las operaciones de autenticación del sistema.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario y genera su sesión JWT.
        /// </summary>
        /// <param name="request">Credenciales de acceso.</param>
        /// <returns>Respuesta de autenticación o null si no se pudo autenticar.</returns>
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string clientIp);
        /// <summary>
        /// No se utiliza actualmente, pero podría ser útil para futuras implementaciones de cierre de sesión.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task LogoutAsync(string sessionId, string username);
    }
}
