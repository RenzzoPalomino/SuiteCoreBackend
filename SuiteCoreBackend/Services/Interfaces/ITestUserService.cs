using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Services.Interfaces
{
    /// <summary>
    /// Define las operaciones temporales para validar usuarios de prueba.
    /// </summary>
    public interface ITestUserService
    {
        /// <summary>
        /// Valida las credenciales ingresadas contra usuarios temporales.
        /// </summary>
        /// <param name="email">Correo electrónico ingresado.</param>
        /// <param name="password">Contraseña ingresada.</param>
        /// <returns>Usuario válido o null si las credenciales no coinciden.</returns>
        TestUser? ValidateCredentials(string email, string password);
    }
}
