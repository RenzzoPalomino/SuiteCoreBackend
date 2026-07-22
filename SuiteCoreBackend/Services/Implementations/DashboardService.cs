using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Dashboard;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DashboardService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/dashboard/summary");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var indicadores = root.TryGetProperty("indicadores", out var indEl)
                ? new DashboardIndicadoresDto
                {
                    AlertasActivas    = indEl.TryGetProperty("alertas_activas",    out var aa) ? aa.GetInt32() : 0,
                    ChecksTotales     = indEl.TryGetProperty("checks_totales",     out var ct) ? ct.GetInt32() : 0,
                    ChecksActivos     = indEl.TryGetProperty("checks_activos",     out var ca) ? ca.GetInt32() : 0,
                    MaquinasVirtuales = indEl.TryGetProperty("maquinas_virtuales", out var mv) ? mv.GetInt32() : 0,
                    Servicios         = indEl.TryGetProperty("servicios",         out var sv) ? sv.GetInt32() : 0
                }
                : new DashboardIndicadoresDto();

            return new DashboardSummaryDto
            {
                Estado = root.GetStringOrEmpty("estado"),
                Indicadores = indicadores
            };
        }

        public async Task<ModulesStatusChartDto> GetModulesStatusChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/dashboard/charts/modules-status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new ModulesStatusDataDto
                {
                    Operativos  = datosEl.TryGetProperty("operativos",  out var op) ? op.GetInt32() : 0,
                    Advertencia = datosEl.TryGetProperty("advertencia", out var ad) ? ad.GetInt32() : 0,
                    Criticos    = datosEl.TryGetProperty("criticos",    out var cr) ? cr.GetInt32() : 0
                }
                : new ModulesStatusDataDto();

            return new ModulesStatusChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public async Task<ServicesStatusChartDto> GetServicesStatusChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/dashboard/charts/services-status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new ServicesStatusDataDto
                {
                    Operativos = datosEl.TryGetProperty("operativos", out var op) ? op.GetInt32() : 0,
                    Caidos     = datosEl.TryGetProperty("caidos",     out var ca) ? ca.GetInt32() : 0
                }
                : new ServicesStatusDataDto();

            return new ServicesStatusChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public Task<HttpResponseMessage> GetAlertsStatusChartRawAsync() =>
            _httpClient.GetAsync(
                "/api/v1/dashboard/charts/alerts-status",
                HttpCompletionOption.ResponseHeadersRead);
    }
}
