using SuiteCoreBackend.DTOs.Alert;
using System.Threading.Tasks;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IAlertService
    {
        /// <summary>
        /// Procesa una alerta recibida vía webhook de Grafana y la notifica por Telegram.
        /// </summary>
        /// <param name="dto">Payload del webhook de Grafana.</param>
        /// <param name="channelId">Id del canal específico a notificar; si es null, notifica a todos los canales activos.</param>
        /// <returns>True si la notificación fue procesada y enviada correctamente.</returns>
        Task<bool> ProcessGrafanaAlertAsync(GrafanaWebhookDto dto, int? channelId);

        /// <summary>
        /// Procesa una alerta recibida vía webhook de LibreNMS y la notifica por Telegram.
        /// </summary>
        /// <param name="dto">Payload del webhook de LibreNMS.</param>
        /// <param name="channelId">Id del canal específico a notificar; si es null, notifica a todos los canales activos.</param>
        /// <returns>True si la notificación fue procesada y enviada correctamente.</returns>
        Task<bool> ProcessLibreNmsAlertAsync(LibreNmsWebhookDto dto, int? channelId);
    }
}
