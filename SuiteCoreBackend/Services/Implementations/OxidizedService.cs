using Microsoft.Extensions.Options;
using SuiteCoreBackend.Helpers;
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

        
        public async Task<OxidizedBackupDto> GetDeviceBackupAsync(
            string deviceName,
            string? oid = null,
            long? epoch = null,
            int? num = null,
            string? group = "")
        {
            var encodedDeviceName = Uri.EscapeDataString(deviceName);
            var url = string.Empty;
            var backupType = "current";

            var isSpecificVersion =
                !string.IsNullOrWhiteSpace(oid) &&
                epoch.HasValue &&
                num.HasValue;

            if (isSpecificVersion)
            {
                var encodedOid = Uri.EscapeDataString(oid!);
                var encodedGroup = Uri.EscapeDataString(group ?? string.Empty);

                url =
                    $"/node/version/view?node={encodedDeviceName}" +
                    $"&group={encodedGroup}" +
                    $"&oid={encodedOid}" +
                    $"&epoch={epoch.Value}" +
                    $"&num={num.Value}"
                    +$"&format=json";

                    backupType = "version";
            }
            else
            {
                url = $"/node/fetch/{encodedDeviceName}";
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"No se pudo obtener la configuración del dispositivo {deviceName}. " +
                    $"Tipo: {backupType}. Status: {(int)response.StatusCode}. Error: {error}"
                );
            }

            var rawContent = await response.Content.ReadAsStringAsync();
            var config = OxidizeHelper.NormalizeOxidizedConfig(rawContent);


            return new OxidizedBackupDto
            {
                DeviceName = deviceName,
                Config = config,
                BackupType = backupType,
                Oid = oid,
                Epoch = epoch,
                Num = num,
                RetrievedAt = DateTime.Now
            };
        }


        
        public async Task<List<OxidizedVersionDto>> GetDeviceVersionsAsync(string deviceName)
        {
            var encodedDeviceName = Uri.EscapeDataString(deviceName);

            var response = await _httpClient.GetAsync(
                $"/node/version?node_full={encodedDeviceName}&format=json"
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"No se pudo obtener el historial de versiones del dispositivo {deviceName}. Status: {(int)response.StatusCode}. Error: {error}"
                );
            }

            var content = await response.Content.ReadAsStringAsync();

            var versions = JsonSerializer.Deserialize<List<OxidizedVersionDto>>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            versions ??= new List<OxidizedVersionDto>();

            var total = versions.Count;

            for (int i = 0; i < versions.Count; i++)
            {
                var version = versions[i];

                version.Epoch = OxidizeHelper.ConvertToEpoch(version.Time ?? version.Date);

                // Según el comportamiento observado en Oxidized:
                // última versión de una lista de 4 usa num=4
                version.Num = total - i;

                if (!string.IsNullOrWhiteSpace(version.Oid) && version.Epoch.HasValue)
                {
                    var backupUrl =
                        $"/api/oxidized/devices/{Uri.EscapeDataString(deviceName)}/backup" +
                        $"?oid={Uri.EscapeDataString(version.Oid)}" +
                        $"&epoch={version.Epoch.Value}" +
                        $"&num={version.Num}";
                    version.BackupUrl = backupUrl;
                }
            }

            return versions;
        }
        
    }
}
