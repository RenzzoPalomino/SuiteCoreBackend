using Flexinets.Radius;
using Flexinets.Radius.Core;
using Microsoft.Extensions.Options;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SuiteCoreBackend.Services.Implementations
{
    public class RadiusSessionService : IRadiusSessionService
    {
        private readonly RadiusSettings _settings;
        private readonly ILogger<RadiusSessionService> _logger;

        public RadiusSessionService(IOptions<RadiusSettings> settings, ILogger<RadiusSessionService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<string> StartSessionAsync(string username, string clientIp)
        {
            var sessionId = Guid.NewGuid().ToString("N")[..16].ToUpper();
            await SendAccountingAsync(username, sessionId, clientIp, 1); // 1 = Start
            return sessionId;
        }

        public async Task StopSessionAsync(string sessionId, string username)
        {
            await SendAccountingAsync(username, sessionId, string.Empty, 2); // 2 = Stop
        }

        private async Task SendAccountingAsync(string username, string sessionId, string clientIp, uint statusType)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(_settings.Server), _settings.AccountingPort);
                var secret = Encoding.UTF8.GetBytes(_settings.SharedSecret);

                using var udpClient = new UdpClient();
                using var client = new RadiusClient(udpClient);

                var packet = RadiusPacket.Create(PacketCode.AccountingRequest, secret);
                packet.AddAttribute("User-Name", username);
                packet.AddAttribute("Acct-Status-Type", statusType);
                packet.AddAttribute("Acct-Session-Id", sessionId);

                if (!string.IsNullOrEmpty(clientIp))
                    packet.AddAttribute("Calling-Station-Id", clientIp);

                await client.SendPacketAsync(packet, endpoint, _settings.TimeoutMs);

                _logger.LogInformation("RADIUS Accounting {Status} enviado para {Username} — sesión {SessionId}",
                    statusType == 1 ? "Start" : "Stop", username, sessionId);
            }
            catch (Exception ex)
            {
                // RADIUS es para tracking, no bloquea el login si falla
                _logger.LogWarning("RADIUS accounting falló para {Username}: {Error}", username, ex.Message);
            }
        }
    }
}
