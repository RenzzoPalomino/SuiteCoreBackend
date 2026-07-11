using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations;

public class GrafanaEmbedService : IGrafanaEmbedService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GrafanaEmbedService(HttpClient httpClient, IOptions<ScnoSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUrl);
    }

    public async Task<IEnumerable<GrafanaEmbedLinkDto>> GetEmbedLinksAsync()
    {
        try
        {
            var json = await _httpClient.GetStringAsync("/api/v1/grafana/embed-links");

            var response = JsonSerializer.Deserialize<ScnoGrafanaEmbedLinksResponse>(json, _jsonOptions);

            var items = response?.EmbedLinks?.Items;
            if (items == null)
            {
                return Enumerable.Empty<GrafanaEmbedLinkDto>();
            }

            return items.Select(r => new GrafanaEmbedLinkDto
            {
                Title = r.Title,
                EmbedUrl = r.EmbedUrl
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los embed links de Grafana desde SCNO", ex);
        }
    }
}
