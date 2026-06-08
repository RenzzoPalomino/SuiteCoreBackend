namespace SuiteCoreBackend.Models.Entities
{
    public class TestUser
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
