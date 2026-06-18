using Flexinets.Radius;
using Flexinets.Radius.Core;
using Microsoft.Extensions.Options;
using SuiteCoreBackend.Enums;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net;

namespace SuiteCoreBackend.Services.Implementations
{
    public class RadiusSessionService : IRadiusSessionService
    {
        private readonly RadiusSettings _settings;
        private readonly ILogger<RadiusSessionService> _logger;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Inicializa el servicio de sesiones RADIUS con la configuración y los loggers necesarios.
        /// </summary>
        /// <param name="settings">Configuración del servidor RADIUS (IP, puerto, secreto compartido).</param>
        /// <param name="logger">Logger del servicio.</param>
        /// <param name="loggerFactory">Fábrica de loggers para instanciar el RadiusPacketParser.</param>
        public RadiusSessionService(
            IOptions<RadiusSettings> settings,
            ILogger<RadiusSessionService> logger,
            ILoggerFactory loggerFactory)
        {
            _settings = settings.Value;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Envía un paquete Accounting-Start a RADIUS para registrar el inicio de sesión del usuario.
        /// </summary>
        /// <param name="username">Nombre de usuario autenticado vía LDAP.</param>
        /// <param name="clientIp">IP del cliente que inicia sesión (Calling-Station-Id).</param>
        /// <returns>Session ID generado (16 caracteres, GUID truncado en mayúsculas).</returns>
        public async Task<string> StartSessionAsync(string username, string clientIp)
        {
            var sessionId = Guid.NewGuid().ToString("N")[..16].ToUpper();
            await SendAccountingAsync(username, sessionId, clientIp, (uint)AccountingStatus.Start);
            return sessionId;
        }

        /// <summary>
        /// Envía un paquete Accounting-Stop a RADIUS para registrar el cierre de sesión del usuario.
        /// </summary>
        /// <param name="sessionId">ID de sesión generado en el login, extraído del JWT.</param>
        /// <param name="username">Nombre de usuario que cierra sesión.</param>
        public async Task StopSessionAsync(string sessionId, string username)
        {
            await SendAccountingAsync(username, sessionId, string.Empty, (uint)AccountingStatus.Stop);
        }

        /// <summary>
        /// Construye y envía un paquete RADIUS Accounting-Request al servidor configurado.
        /// Si RADIUS no está disponible, registra un warning sin lanzar excepción para no bloquear el flujo de autenticación.
        /// </summary>
        /// <param name="username">Nombre de usuario incluido en el atributo User-Name.</param>
        /// <param name="sessionId">ID de sesión incluido en el atributo Acct-Session-Id.</param>
        /// <param name="clientIp">IP del cliente incluida en Calling-Station-Id. Vacío en Stop.</param>
        /// <param name="statusType">Tipo de accounting: 1 = Start, 2 = Stop (AccountingStatus enum).</param>
        private async Task SendAccountingAsync(string username, string sessionId, string clientIp, uint statusType)
        {
            try
            {
                var remoteEndpoint = new IPEndPoint(IPAddress.Parse(_settings.Server), _settings.AccountingPort);
                var localEndpoint = new IPEndPoint(IPAddress.Any, 0);

                using var client = new RadiusClient(
                    localEndpoint,
                    new RadiusPacketParser(
                        _loggerFactory.CreateLogger<RadiusPacketParser>(),
                        RadiusDictionary.Parse(DefaultDictionary.RadiusDictionary)));

                var packet = new RadiusPacket(PacketCode.AccountingRequest, 0, _settings.SharedSecret);
                //var packet = new RadiusPacket(PacketCode.AccountingRequest, (byte)Random.Shared.Next(0, 256), _settings.SharedSecret);
                packet.AddAttribute("User-Name", username);
                packet.AddAttribute("Acct-Status-Type", statusType);
                packet.AddAttribute("Acct-Session-Id", sessionId);
                packet.AddAttribute("NAS-Identifier", "SuiteCoreBackend");

                //if (!string.IsNullOrEmpty(clientIp))
                //    packet.AddAttribute("Calling-Station-Id", clientIp);

                var timeout = TimeSpan.FromMilliseconds(_settings.TimeoutMs);
                await client.SendPacketAsync(packet, remoteEndpoint, timeout);

                _logger.LogInformation($"RADIUS Accounting Status enviado para {username} — sesión {sessionId}",
                    statusType == (uint)AccountingStatus.Start ? "Start" : "Stop", username, sessionId);
            }
            catch (Exception ex)
            {
                // RADIUS es para tracking, no bloquea el login si falla
                _logger.LogWarning("RADIUS accounting falló para {Username}: {Error}", username, ex.Message);
            }
        }
    }
}
