using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface IGrafanaEmbedService
{
    /// <summary>
    /// Obtiene el listado de embed links de Grafana consultando la API externa de SCNO.
    /// </summary>
    /// <returns>Colección de embed links (título + URL) para incrustar paneles de Grafana.</returns>
    Task<IEnumerable<GrafanaEmbedLinkDto>> GetEmbedLinksAsync();
}
