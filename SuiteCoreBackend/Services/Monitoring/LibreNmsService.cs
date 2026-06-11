using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Services.Interfaces;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Monitoring;

public class LibreNmsService : ILibreNmsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public LibreNmsService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<IEnumerable<DeviceTypeDto>> GetDeviceTypesAsync()
    {
        var url = _config["LibreNMS:Url"];
        var token = _config["LibreNMS:Token"];

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{url}/api/v0/devices"
        );

        request.Headers.Add("X-Auth-Token", token);

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        using var data = JsonDocument.Parse(json);

        var devices = data.RootElement
            .GetProperty("devices")
            .EnumerateArray()
            .ToList();

        var result = devices
            .GroupBy(d => d.GetProperty("type").GetString())
            .Select(g => new DeviceTypeDto
            {
                Type = g.Key ?? "unknown",
                Count = g.Count()
            })
            .ToList();

        return result;
    }
}
