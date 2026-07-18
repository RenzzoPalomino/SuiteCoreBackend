using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Infrastructure;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class InfrastructureService : IInfrastructureService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public InfrastructureService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<InfrastructureSummaryDto> GetSummaryAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/infrastructure/summary");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new InfrastructureSummaryDto
            {
                Estado            = root.GetStringOrEmpty("estado"),
                NodosActivos      = root.TryGetProperty("nodos_activos",      out var na) ? na.GetInt32()  : 0,
                MaquinasVirtuales = root.TryGetProperty("maquinas_virtuales", out var mv) ? mv.GetInt32()  : 0,
                MemoriaUso        = root.TryGetProperty("memoria_uso",        out var mu) ? mu.GetDouble() : 0,
                AlmacenamientoUso = root.TryGetProperty("almacenamiento_uso", out var au) ? au.GetDouble() : 0
            };
        }

        public async Task<ResourcesUsageChartDto> GetResourcesUsageChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/infrastructure/charts/resources");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new ResourcesUsageDataDto
                {
                    Cpu             = datosEl.TryGetProperty("cpu",             out var cpu) ? cpu.GetDouble() : 0,
                    Memoria         = datosEl.TryGetProperty("memoria",         out var mem) ? mem.GetDouble() : 0,
                    Almacenamiento  = datosEl.TryGetProperty("almacenamiento",  out var alm) ? alm.GetDouble() : 0
                }
                : new ResourcesUsageDataDto();

            return new ResourcesUsageChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }
    }
}
