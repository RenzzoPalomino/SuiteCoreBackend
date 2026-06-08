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
        LoginResponseDto? Login(LoginRequestDto request);
    }
}
