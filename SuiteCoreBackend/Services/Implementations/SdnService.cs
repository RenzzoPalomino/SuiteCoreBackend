using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using SuiteCoreBackend.DTOs.Sdn;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class SdnService : ISdnService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SdnService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<SdnHealthDto> GetHealthAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/health");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new SdnHealthDto
            {
                Status     = root.GetStringOrEmpty("status"),
                Service    = root.GetStringOrEmpty("service"),
                Version    = root.GetStringOrEmpty("version"),
                Controller = root.GetStringOrEmpty("controller"),
                Bridge     = root.GetStringOrEmpty("bridge"),
                Openflow   = root.GetStringOrEmpty("openflow"),
                Timestamp  = root.GetStringOrEmpty("timestamp")
            };
        }

        public async Task<SdnTopologyDto> GetTopologyAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/topology");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var ports = root.TryGetProperty("ports", out var portsEl)
                ? portsEl.EnumerateArray().Select(p => p.GetString() ?? string.Empty).ToList()
                : new List<string>();

            return new SdnTopologyDto
            {
                Status     = root.GetStringOrEmpty("status"),
                Bridge     = root.GetStringOrEmpty("bridge"),
                Controller = root.GetStringOrEmpty("controller"),
                OvsVersion = root.GetStringOrEmpty("ovs_version"),
                Ports      = ports
            };
        }

        public async Task<SdnStatisticsDto> GetStatisticsAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/statistics");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var ports = root.TryGetProperty("ports", out var portsEl)
                ? JsonSerializer.Deserialize<List<object>>(portsEl.GetRawText(), _jsonOptions) ?? new()
                : new List<object>();

            return new SdnStatisticsDto
            {
                Status = root.GetStringOrEmpty("status"),
                Ports  = ports
            };
        }

        public async Task<SdnFlowsDto> GetFlowsAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/flows");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var flows = root.TryGetProperty("flows", out var flowsEl)
                ? JsonSerializer.Deserialize<List<object>>(flowsEl.GetRawText(), _jsonOptions) ?? new()
                : new List<object>();

            return new SdnFlowsDto
            {
                Status    = root.GetStringOrEmpty("status"),
                FlowCount = root.TryGetProperty("flow_count", out var fc) ? fc.GetInt32() : 0,
                Flows     = flows
            };
        }

        public async Task<MikroTikInfoDto> GetMikroTikInfoAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/mikrotik/info");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new MikroTikInfoDto
            {
                Status       = root.GetStringOrEmpty("status"),
                Identity     = root.GetStringOrEmpty("identity"),
                Version      = root.GetStringOrEmpty("version"),
                Platform     = root.GetStringOrEmpty("platform"),
                Cpu          = root.GetStringOrEmpty("cpu"),
                CpuLoad      = root.TryGetProperty("cpu_load",     out var cl)  ? cl.GetInt32()   : 0,
                Architecture = root.GetStringOrEmpty("architecture"),
                Uptime       = root.GetStringOrEmpty("uptime"),
                TotalMemory  = root.TryGetProperty("total_memory", out var tm)  ? tm.GetInt64()   : 0,
                FreeMemory   = root.TryGetProperty("free_memory",  out var fm)  ? fm.GetInt64()   : 0
            };
        }

        public async Task<MikroTikInterfacesDto> GetMikroTikInterfacesAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/mikrotik/interfaces");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var interfaces = new List<MikroTikInterfaceItemDto>();
            if (root.TryGetProperty("interfaces", out var ifacesEl))
            {
                foreach (var iface in ifacesEl.EnumerateArray())
                {
                    interfaces.Add(new MikroTikInterfaceItemDto
                    {
                        Name     = iface.GetStringOrEmpty("name"),
                        Type     = iface.GetStringOrEmpty("type"),
                        Running  = iface.TryGetProperty("running",  out var r) && r.GetBoolean(),
                        Disabled = iface.TryGetProperty("disabled", out var d) && d.GetBoolean(),
                        Comment  = iface.GetStringOrEmpty("comment")
                    });
                }
            }

            return new MikroTikInterfacesDto
            {
                Status     = root.GetStringOrEmpty("status"),
                Interfaces = interfaces
            };
        }

        public async Task<object> BlockIpAsync(SdnBlockIpDto request)
        {
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "/api/v1/security/block-ip")
            {
                Content = JsonContent.Create(new
                {
                    ip = request.Ip
                })
            };

            addHeaders(httpRequest);

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<object>() ?? new { };
        }


        public async Task<object> UnblockIpAsync(SdnBlockIpDto request)
        {
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Delete,
                "/api/v1/security/block-ip")
            {
                Content = JsonContent.Create(new
                {
                    ip = request.Ip
                })
            };

            addHeaders(httpRequest);

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<object>() ?? new { };
        }

        public async Task<object> ExecuteAutomationAsync(SdnAutomationRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/automation/execute", new { action = request.Action });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<object>() ?? new { };
        }

        public async Task<object> ForwardGraylogWebhookAsync(JsonElement payload)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/webhooks/graylog", payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<object>() ?? new { };
        }

        void addHeaders(HttpRequestMessage rq)
        {
            rq.Headers.Add("X-SCNO-User", _settings.User);
            rq.Headers.Add("X-SCNO-Role", _settings.Role);
        }
    }

    // Extension local para reducir verbosidad al leer strings de JsonElement
    internal static class JsonElementExtensions
    {
        internal static string GetStringOrEmpty(this JsonElement el, string property) =>
            el.TryGetProperty(property, out var prop) ? prop.GetString() ?? string.Empty : string.Empty;
    }
}
