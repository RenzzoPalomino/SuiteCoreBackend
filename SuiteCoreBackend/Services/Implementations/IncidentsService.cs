using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Incidents;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class IncidentsService : IIncidentsService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public IncidentsService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<IncidentsSummaryDto> GetSummaryAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/incidents/summary");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var indicadores = new IncidentsIndicadoresDto();
            if (root.TryGetProperty("indicadores", out var indEl))
            {
                indicadores.AlertasActivas = indEl.TryGetProperty("alertas_activas", out var aa) ? aa.GetInt32() : 0;
                indicadores.IncidentesHistoricos = indEl.TryGetProperty("incidentes_historicos", out var ih) ? ih.GetInt32() : 0;

                if (indEl.TryGetProperty("notificaciones", out var notifEl))
                {
                    indicadores.Notificaciones = new IncidentsNotificacionesDto
                    {
                        Telegram = notifEl.TryGetProperty("telegram", out var tg) && tg.GetBoolean(),
                        Correo   = notifEl.TryGetProperty("correo",   out var co) && co.GetBoolean()
                    };
                }
            }

            return new IncidentsSummaryDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Estado = root.GetStringOrEmpty("estado"),
                Indicadores = indicadores
            };
        }

        public async Task<IncidentsSeverityChartDto> GetSeverityChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/incidents/charts/severity");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new IncidentsSeverityDataDto
                {
                    Criticas      = datosEl.TryGetProperty("criticas",      out var c) ? c.GetInt32() : 0,
                    Advertencias  = datosEl.TryGetProperty("advertencias",  out var a) ? a.GetInt32() : 0,
                    Informativas  = datosEl.TryGetProperty("informativas",  out var i) ? i.GetInt32() : 0
                }
                : new IncidentsSeverityDataDto();

            return new IncidentsSeverityChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public async Task<IncidentsModulesChartDto> GetModulesChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/incidents/charts/modules");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = new Dictionary<string, int>();
            if (root.TryGetProperty("datos", out var datosEl))
            {
                foreach (var prop in datosEl.EnumerateObject())
                {
                    datos[prop.Name] = prop.Value.ValueKind == JsonValueKind.Number ? prop.Value.GetInt32() : 0;
                }
            }

            return new IncidentsModulesChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public Task<GraylogEventsResultDto> GetEventsAsync() =>
            GetGraylogResultAsync("/api/v1/incidents/events");

        public Task<GraylogEventsResultDto> GetSecurityEventsAsync() =>
            GetGraylogResultAsync("/api/v1/incidents/security-events");

        private async Task<GraylogEventsResultDto> GetGraylogResultAsync(string path)
        {
            var json = await _httpClient.GetStringAsync(path);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var items = new List<GraylogEventItemDto>();
            if (root.TryGetProperty("items", out var itemsEl))
            {
                foreach (var item in itemsEl.EnumerateArray())
                {
                    var streams = item.TryGetProperty("streams", out var streamsEl)
                        ? streamsEl.EnumerateArray().Select(s => s.GetString() ?? string.Empty).ToList()
                        : new List<string>();

                    items.Add(new GraylogEventItemDto
                    {
                        Id              = item.GetStringOrEmpty("id"),
                        Index           = item.GetStringOrEmpty("index"),
                        Timestamp       = item.GetStringOrEmpty("timestamp"),
                        Source          = item.GetStringOrEmpty("source"),
                        Level           = item.TryGetProperty("level", out var lvl) ? lvl.GetInt32() : 0,
                        Facility        = item.GetStringOrEmpty("facility"),
                        ApplicationName = item.GetStringOrEmpty("application_name"),
                        Message         = item.GetStringOrEmpty("message"),
                        FullMessage     = item.GetStringOrEmpty("full_message"),
                        RemoteIp        = item.GetStringOrEmpty("remote_ip"),
                        Streams         = streams
                    });
                }
            }

            return new GraylogEventsResultDto
            {
                Success       = root.TryGetProperty("success", out var s) && s.GetBoolean(),
                Service       = root.GetStringOrEmpty("service"),
                Query         = root.GetStringOrEmpty("query"),
                BuiltQuery    = root.GetStringOrEmpty("built_query"),
                RangeSeconds  = root.TryGetProperty("range_seconds",  out var rs) ? rs.GetInt32() : 0,
                Limit         = root.TryGetProperty("limit",          out var lm) ? lm.GetInt32() : 0,
                TotalResults  = root.TryGetProperty("total_results",  out var tr) ? tr.GetInt32() : 0,
                MessagesCount = root.TryGetProperty("messages_count", out var mc) ? mc.GetInt32() : 0,
                Items         = items
            };
        }
    }
}
