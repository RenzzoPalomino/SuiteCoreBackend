namespace SuiteCoreBackend.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserSessionDto User { get; set; } = new();
    }
}
