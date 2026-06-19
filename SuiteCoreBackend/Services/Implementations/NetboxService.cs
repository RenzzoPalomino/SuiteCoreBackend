using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Services.Monitoring;

public class NetboxService : INetboxService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public NetboxService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxRegionDto>();
            }

            return netboxResponse.Results.Select(r => new NetboxRegionDto
            {
                Name = r.Name,
                SiteCount = r.SiteCount,
                Description = r.Description
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las regiones desde Netbox", ex);
        }
    }
}
