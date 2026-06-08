using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
namespace SuiteCoreBackend.Services
{
    /// <summary>
    /// Servicio temporal para simular usuarios durante la implementación inicial de autenticación.
    /// </summary>
    public class TestUserService : ITestUserService
    {
        private static readonly List<TestUser> Users = new()
        {
            new TestUser
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                NombreCompleto = "Usuario Administrador",
                Email = "admin@suitecore.com",
                Password = "123456",
                Rol = "Administrador",
                Activo = true
            },
            new TestUser
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                NombreCompleto = "Usuario Asesor",
                Email = "asesor@suitecore.com",
                Password = "123456",
                Rol = "Asesor",
                Activo = true
            },
            new TestUser
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                NombreCompleto = "Usuario Inactivo",
                Email = "inactivo@suitecore.com",
                Password = "123456",
                Rol = "Asesor",
                Activo = false
            }
        };

        /// <summary>
        /// Valida el correo y contraseña contra la lista temporal de usuarios.
        /// </summary>
        /// <param name="email">Correo electrónico ingresado.</param>
        /// <param name="password">Contraseña ingresada.</param>
        /// <returns>Usuario encontrado o null si no existe coincidencia.</returns>
        public TestUser? ValidateCredentials(string email, string password)
        {
            return Users.FirstOrDefault(x =>
                x.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Password == password
            );
        }
    }
}
