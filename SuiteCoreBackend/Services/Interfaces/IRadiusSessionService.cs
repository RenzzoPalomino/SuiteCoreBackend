namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IRadiusSessionService
    {
        Task StartSessionAsync(string username, string clientIp,string sessionId);
        Task StopSessionAsync(string sessionId, string username);
    }
}
