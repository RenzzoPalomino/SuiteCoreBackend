using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Backup;
using SuiteCoreBackend.DTOs.Oxidized;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class OxidizedService : IOxidizedService
    {
        private readonly HttpClient _httpClient;
        private readonly OxidizedSettings _settings;

        public OxidizedService(
            HttpClient httpClient,
            IOptions<OxidizedSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);

            var credentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}")
            );

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
        }

        public async Task<List<OxidizedDeviceDto>> GetDevicesAsync()
        {
            var response = await _httpClient.GetAsync("/nodes.json");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"No se pudo obtener dispositivos desde Oxidized. Status: {(int)response.StatusCode}. Error: {error}"
                );
            }

            var content = await response.Content.ReadAsStringAsync();

            var devices = JsonSerializer.Deserialize<List<OxidizedDeviceDto>>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return devices ?? new List<OxidizedDeviceDto>();
        }

        public async Task<OxidizedBackupDto> GetDeviceBackupAsync(string deviceName)
        {
            //var response = await _httpClient.GetAsync($"/node/fetch/{deviceName}");

            var encodedDeviceName = Uri.EscapeDataString(deviceName);
            var response = await _httpClient.GetAsync($"/node/fetch/{encodedDeviceName}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"No se pudo obtener el backup del dispositivo {deviceName}. Status: {(int)response.StatusCode}. Error: {error}"
                );
            }

            var config = await response.Content.ReadAsStringAsync();

            return new OxidizedBackupDto
            {
                DeviceName = deviceName,
                Config = config,
                RetrievedAt = DateTime.Now
            };
        }

        public async Task<List<OxidizedBackupDto>> GetAllDeviceBackupsAsync()
        {
            var devices = await GetDevicesAsync();

            var backups = new List<OxidizedBackupDto>();

            foreach (var device in devices)
            {
                if (string.IsNullOrWhiteSpace(device.Name))
                    continue;

                try
                {
                    var backup = await GetDeviceBackupAsync(device.Name);
                    backups.Add(backup);
                }
                catch (Exception ex)
                {
                    backups.Add(new OxidizedBackupDto
                    {
                        DeviceName = device.Name,
                        Config = $"ERROR: No se pudo obtener el backup. Detalle: {ex.Message}",
                        RetrievedAt = DateTime.Now
                    });
                }
            }

            return backups;
        }
    }
}
