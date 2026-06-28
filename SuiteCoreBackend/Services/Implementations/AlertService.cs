using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SuiteCoreBackend.DTOs.Alert;
using SuiteCoreBackend.Helpers;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Implementations
{
    public class AlertService : IAlertService
    {
        private readonly IAlertEventRepository _alertRepository;
        private readonly INotificationChannelRepository _channelRepository;
        private readonly HttpClient _httpClient;
        private readonly DateTimeHelper _dateHelper = new DateTimeHelper();

        public AlertService(
            IAlertEventRepository alertRepository,
            INotificationChannelRepository channelRepository,
            HttpClient httpClient)
        {
            _alertRepository = alertRepository;
            _channelRepository = channelRepository;
            _httpClient = httpClient;
        }

        public async Task<bool> ProcessGrafanaAlertAsync(GrafanaWebhookDto dto, int? channelId)
        {
            try
            {
                var success = true;

                // Si Grafana envía alertas en la lista, procesamos cada una
                if (dto.Alerts != null && dto.Alerts.Count > 0)
                {
                    foreach (var alertItem in dto.Alerts)
                    {
                        alertItem.Labels.TryGetValue("alertname", out var alertName);
                        alertItem.Labels.TryGetValue("instance", out var instance);
                        alertItem.Labels.TryGetValue("severity", out var severity);
                        
                        alertItem.Annotations.TryGetValue("description", out var description);
                        if (string.IsNullOrEmpty(description))
                        {
                            alertItem.Annotations.TryGetValue("summary", out description);
                        }

                        var alertEvent = new AlertEvent
                        {
                            Source = "Grafana",
                            Title = alertName ?? "Alerta de Grafana",
                            Status = alertItem.Status,
                            Severity = severity ?? "warning",
                            Instance = instance,
                            Description = description,
                            CreatedDate = DateTime.UtcNow
                        };

                        // Persistir en base de datos
                        await _alertRepository.CreateAsync(alertEvent);

                        // Enviar a Telegram
                        var sendOk = await SendAlertToTelegramAsync(alertEvent, channelId);
                        if (!sendOk) success = false;
                    }
                }
                else
                {
                    // Si viene un payload genérico/vacío de alertas, creamos uno base
                    var alertEvent = new AlertEvent
                    {
                        Source = "Grafana",
                        Title = "Alerta de Grafana",
                        Status = dto.Status ?? "firing",
                        Severity = "warning",
                        CreatedDate = DateTime.UtcNow
                    };

                    await _alertRepository.CreateAsync(alertEvent);
                    success = await SendAlertToTelegramAsync(alertEvent, channelId);
                }

                return success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ProcessLibreNmsAlertAsync(LibreNmsWebhookDto dto, int? channelId)
        {
            try
            {
                var status = dto.State == 1 ? "firing" : (dto.State == 0 ? "resolved" : $"state-{dto.State}");

                var alertEvent = new AlertEvent
                {
                    Source = "LibreNMS",
                    Title = dto.Title,
                    Status = status,
                    Severity = dto.Severity ?? "critical",
                    Instance = dto.Hostname,
                    Description = dto.Msg,
                    CreatedDate = DateTime.UtcNow
                };

                // Persistir en base de datos
                await _alertRepository.CreateAsync(alertEvent);

                // Enviar a Telegram
                return await SendAlertToTelegramAsync(alertEvent, channelId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SendAlertToTelegramAsync(AlertEvent alertEvent, int? channelId)
        {
            var channels = new List<NotificationChannel>();

            // Enrutamiento mixto
            if (channelId.HasValue)
            {
                var channel = await _channelRepository.GetByIdAsync(channelId.Value);
                if (channel != null && channel.IsActive && channel.Type.ToLower() == "telegram")
                {
                    channels.Add(channel);
                }
            }

            // Si no se especificó un canal válido o activo, buscamos todos los activos de tipo telegram
            if (channels.Count == 0)
            {
                var allChannels = await _channelRepository.GetAllAsync();
                foreach (var ch in allChannels)
                {
                    if (ch.IsActive && ch.Type.ToLower() == "telegram")
                    {
                        channels.Add(ch);
                    }
                }
            }

            if (channels.Count == 0)
            {
                return false;
            }

            var messageText = FormatTelegramMessage(alertEvent);
            var overallSuccess = true;

            foreach (var channel in channels)
            {
                var success = await SendTelegramPayloadAsync(channel.BotToken, channel.ChatId, messageText);
                if (!success)
                {
                    overallSuccess = false;
                }
            }

            return overallSuccess;
        }

        private async Task<bool> SendTelegramPayloadAsync(string botToken, string chatId, string message)
        {
            try
            {
                var payload = new
                {
                    chat_id = chatId,
                    text = message,
                    parse_mode = "HTML"
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
                var response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private string FormatTelegramMessage(AlertEvent alert)
        {
            var isResolved = alert.Status.ToLower() == "resolved";
            var emoji = isResolved ? "✅" : (alert.Severity?.ToLower() == "critical" ? "🚨" : "⚠️");
            var titleHeader = isResolved ? "ALERTA RESUELTA" : "ALERTA DETECTADA";
            var estadoText = isResolved ? "Resolved 🟢" : (alert.Status.ToLower() == "firing" ? "Firing 🔥" : alert.Status);
            var dateStr = _dateHelper.GetPeruDateTime().ToString("dd/MM/yyyy HH:mm");

            var builder = new StringBuilder();
            builder.AppendLine($"{emoji} <b>{titleHeader} - {alert.Source}</b>");
            builder.AppendLine();
            builder.AppendLine($"<b>Alerta:</b> {alert.Title}");
            builder.AppendLine($"<b>Estado:</b> {estadoText}");

            if (!string.IsNullOrEmpty(alert.Severity))
                builder.AppendLine($"<b>Gravedad:</b> {alert.Severity}");

            if (!string.IsNullOrEmpty(alert.Instance))
                builder.AppendLine($"<b>Instancia:</b> {alert.Instance}");

            if (!string.IsNullOrEmpty(alert.Description))
                builder.AppendLine($"<b>Descripción:</b> {alert.Description}");

            builder.AppendLine($"<b>Fecha:</b> {dateStr}");
            builder.AppendLine();
            builder.AppendLine("<i>Sistema de Monitoreo Operativo.</i>");

            return builder.ToString();
        }
    }
}
