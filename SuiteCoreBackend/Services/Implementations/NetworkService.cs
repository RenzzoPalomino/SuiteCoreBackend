using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Network;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class NetworkService : INetworkService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public NetworkService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<NetworkSummaryDto> GetSummaryAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/summary");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new NetworkSummaryDto
            {
                Estado              = root.GetStringOrEmpty("estado"),
                Dispositivos        = root.TryGetProperty("dispositivos",         out var d)  ? d.GetInt32()  : 0,
                DispositivosActivos = root.TryGetProperty("dispositivos_activos", out var da) ? da.GetInt32() : 0,
                Interfaces          = root.TryGetProperty("interfaces",           out var i)  ? i.GetInt32()  : 0,
                AlertasActivas      = root.TryGetProperty("alertas_activas",      out var aa) ? aa.GetInt32() : 0
            };
        }

        public async Task<NetworkAlertsStatusChartDto> GetAlertsStatusChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/charts/alerts-status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new NetworkAlertsStatusDataDto
                {
                    Criticas     = datosEl.TryGetProperty("criticas",     out var c) ? c.GetInt32() : 0,
                    Advertencias = datosEl.TryGetProperty("advertencias", out var w) ? w.GetInt32() : 0
                }
                : new NetworkAlertsStatusDataDto();

            return new NetworkAlertsStatusChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public async Task<NetworkDevicesStatusChartDto> GetDevicesStatusChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/charts/devices-status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new NetworkDevicesStatusDataDto
                {
                    Activos = datosEl.TryGetProperty("activos", out var a) ? a.GetInt32() : 0,
                    Caidos  = datosEl.TryGetProperty("caidos",  out var c) ? c.GetInt32() : 0
                }
                : new NetworkDevicesStatusDataDto();

            return new NetworkDevicesStatusChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public async Task<NetworkInterfacesStatusChartDto> GetInterfacesStatusChartAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/charts/interfaces-status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? new NetworkInterfacesStatusDataDto
                {
                    Activas   = datosEl.TryGetProperty("activas",   out var a) ? a.GetInt32() : 0,
                    Inactivas = datosEl.TryGetProperty("inactivas", out var i) ? i.GetInt32() : 0
                }
                : new NetworkInterfacesStatusDataDto();

            return new NetworkInterfacesStatusChartDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Tipo = root.GetStringOrEmpty("tipo"),
                Datos = datos
            };
        }

        public async Task<NetworkAlertsTableDto> GetAlertsTableAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/tables/alerts");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var datos = root.TryGetProperty("datos", out var datosEl)
                ? JsonSerializer.Deserialize<List<object>>(datosEl.GetRawText()) ?? new()
                : new List<object>();

            return new NetworkAlertsTableDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Datos = datos
            };
        }

        public async Task<NetworkDevicesTableDto> GetDevicesTableAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/tables/devices");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var devices = new List<NetworkDeviceDto>();
            if (root.TryGetProperty("datos", out var datosEl))
            {
                foreach (var dev in datosEl.EnumerateArray())
                {
                    devices.Add(new NetworkDeviceDto
                    {
                        DeviceId    = dev.TryGetProperty("device_id", out var did) ? did.GetInt32() : 0,
                        Hostname    = dev.GetStringOrEmpty("hostname"),
                        Display     = dev.GetStringOrEmpty("display"),
                        SysName     = dev.GetStringOrEmpty("sysName"),
                        Ip          = dev.GetStringOrEmpty("ip"),
                        Os          = dev.GetStringOrEmpty("os"),
                        Type        = dev.GetStringOrEmpty("type"),
                        Status      = dev.TryGetProperty("status", out var st) ? st.GetInt32() : 0,
                        StatusLabel = dev.GetStringOrEmpty("status_label"),
                        Uptime      = dev.TryGetProperty("uptime", out var up) ? up.GetInt64() : 0,
                        LastPolled  = dev.GetStringOrEmpty("last_polled"),
                        LastPing    = dev.GetStringOrEmpty("last_ping"),
                        Location    = dev.GetStringOrEmpty("location")
                    });
                }
            }

            return new NetworkDevicesTableDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Datos = devices
            };
        }

        public async Task<NetworkInterfacesTableDto> GetInterfacesTableAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/network/tables/interfaces");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var interfaces = new List<NetworkInterfaceDto>();
            if (root.TryGetProperty("datos", out var datosEl))
            {
                foreach (var iface in datosEl.EnumerateArray())
                {
                    interfaces.Add(new NetworkInterfaceDto
                    {
                        PortId        = iface.TryGetProperty("port_id", out var pid) ? pid.GetInt32() : 0,
                        IfName        = iface.GetStringOrEmpty("ifName"),
                        IfDescr       = iface.TryGetProperty("ifDescr", out var descr) && descr.ValueKind == JsonValueKind.String ? descr.GetString() : null,
                        IfAlias       = iface.TryGetProperty("ifAlias", out var alias) && alias.ValueKind == JsonValueKind.String ? alias.GetString() : null,
                        IfOperStatus  = iface.TryGetProperty("ifOperStatus", out var oper) && oper.ValueKind == JsonValueKind.String ? oper.GetString() : null,
                        IfAdminStatus = iface.TryGetProperty("ifAdminStatus", out var admin) && admin.ValueKind == JsonValueKind.String ? admin.GetString() : null,
                        DeviceId      = iface.TryGetProperty("device_id", out var did) && did.ValueKind == JsonValueKind.Number ? did.GetInt32() : (int?)null
                    });
                }
            }

            return new NetworkInterfacesTableDto
            {
                Titulo = root.GetStringOrEmpty("titulo"),
                Datos = interfaces
            };
        }
    }
}
