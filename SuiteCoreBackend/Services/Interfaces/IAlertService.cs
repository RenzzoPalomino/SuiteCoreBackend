using System.Threading.Tasks;
using SuiteCoreBackend.DTOs.Alert;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IAlertService
    {
        Task<bool> ProcessGrafanaAlertAsync(GrafanaWebhookDto dto, int? channelId);
        Task<bool> ProcessLibreNmsAlertAsync(LibreNmsWebhookDto dto, int? channelId);
    }
}
