namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IRadiusSessionService
    {
        Task<string> StartSessionAsync(string username, string clientIp);
        Task StopSessionAsync(string sessionId, string username);
    }
}
