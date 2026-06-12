using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public LdapUserDto User { get; set; } = new();
    }
}
