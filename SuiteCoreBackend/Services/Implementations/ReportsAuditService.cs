using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.ReportsAudit;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class ReportsAuditService : IReportsAuditService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public ReportsAuditService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<AlertsHistoryDto> GetAlertsHistoryAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/alerts/history");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var events = new List<AlertHistoryEventDto>();
            if (root.TryGetProperty("events", out var eventsEl))
            {
                foreach (var evt in eventsEl.EnumerateArray())
                {
                    var confirmation = new AlertConfirmationDto();
                    if (evt.TryGetProperty("confirmation", out var confEl))
                    {
                        confirmation = new AlertConfirmationDto
                        {
                            Observations = confEl.TryGetProperty("observations", out var ob) ? ob.GetInt32() : 0,
                            Required     = confEl.TryGetProperty("required",     out var rq) ? rq.GetInt32() : 0
                        };
                    }

                    var notifications = new List<AlertNotificationResultDto>();
                    if (evt.TryGetProperty("notifications", out var notifsEl))
                    {
                        foreach (var notif in notifsEl.EnumerateArray())
                        {
                            notifications.Add(new AlertNotificationResultDto
                            {
                                Channel = notif.GetStringOrEmpty("channel"),
                                Ok      = notif.TryGetProperty("ok", out var ok) && ok.GetBoolean(),
                                Message = notif.GetStringOrEmpty("message")
                            });
                        }
                    }

                    events.Add(new AlertHistoryEventDto
                    {
                        Timestamp    = evt.GetStringOrEmpty("timestamp"),
                        Service      = evt.GetStringOrEmpty("service"),
                        Module       = evt.GetStringOrEmpty("module"),
                        Priority     = evt.GetStringOrEmpty("priority"),
                        Event        = evt.GetStringOrEmpty("event"),
                        State        = evt.GetStringOrEmpty("state"),
                        Detail       = evt.GetStringOrEmpty("detail"),
                        Elapsed      = evt.GetStringOrEmpty("elapsed"),
                        Confirmation = confirmation,
                        Notifications = notifications
                    });
                }
            }

            return new AlertsHistoryDto
            {
                Success = root.TryGetProperty("success", out var suc) && suc.GetBoolean(),
                Service = root.GetStringOrEmpty("service"),
                Count   = root.TryGetProperty("count", out var cnt) ? cnt.GetInt32() : 0,
                Limit   = root.TryGetProperty("limit", out var lim) ? lim.GetInt32() : 0,
                Events  = events
            };
        }

        public async Task<AutomationAuditStatusDto> GetAuditStatusAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/automation/audit/status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new AutomationAuditStatusDto
            {
                Status         = root.GetStringOrEmpty("status"),
                Database       = root.GetStringOrEmpty("database"),
                Integrity      = root.GetStringOrEmpty("integrity"),
                JournalMode    = root.GetStringOrEmpty("journal_mode"),
                ExecutionCount = root.TryGetProperty("execution_count", out var ec) ? ec.GetInt32() : 0
            };
        }
    }
}
