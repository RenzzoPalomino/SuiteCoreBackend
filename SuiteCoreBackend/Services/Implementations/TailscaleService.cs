using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Vpn;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class TailscaleService : ITailscaleService
    {
        private readonly HttpClient _httpClient;
        private readonly TailscaleSettings _settings;

        private const string TailscaleApiBase = "https://api.tailscale.com/api/v2";

        public TailscaleService(HttpClient httpClient, IOptions<TailscaleSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<TailscaleStatusDto> GetStatusAsync()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{TailscaleApiBase}/tailnet/{_settings.Tailnet}/devices"
            );
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var devices = doc.RootElement
                .GetProperty("devices")
                .EnumerateArray()
                .ToList();

            // El nodo local es el que está en el mismo host que el servidor VPN
            var localNode = devices.FirstOrDefault(d =>
                d.GetProperty("hostname").GetString()?.Contains("security-core") == true);

            var meshIp = localNode.ValueKind != JsonValueKind.Undefined
                ? localNode.GetProperty("addresses").EnumerateArray()
                    .FirstOrDefault().GetString() ?? string.Empty
                : string.Empty;

            var peers = devices
                .Select(d => MapPeer(d))
                .OrderByDescending(p => p.IsOnline)
                .ThenBy(p => p.Hostname)
                .ToList();

            return new TailscaleStatusDto
            {
                IsConnected = !string.IsNullOrEmpty(meshIp),
                MeshIp = meshIp,
                NodeName = localNode.ValueKind != JsonValueKind.Undefined
                    ? localNode.GetProperty("hostname").GetString() ?? string.Empty
                    : string.Empty,
                DerpActive = peers.Any(p => p.IsOnline),
                Peers = peers
            };
        }

        public VpnAccessPolicyDto GetAccessPolicy() => new VpnAccessPolicyDto
        {
            Origin = "172.16.40.0/24",
            Destinations = new List<AccessDestinationDto>
            {
                new() { Network = "172.16.20.0/24", IsAllowed = true },
                new() { Network = "172.16.30.0/24", IsAllowed = true },
                new() { Network = "172.16.50.0/24", IsAllowed = true },
                new() { Network = "172.16.80.0/24", IsAllowed = true },
                new() { Network = "Internet",        IsAllowed = true }
            }
        };

        // ── Helpers ──────────────────────────────────────────────────────────────

        private static TailscalePeerDto MapPeer(JsonElement device)
        {
            var lastSeenRaw = device.TryGetProperty("lastSeen", out var ls)
                ? ls.GetString() : null;

            var isOnline = device.TryGetProperty("online", out var onlineProp)
                && onlineProp.GetBoolean();

            var lastSeen = isOnline ? "active" : FormatLastSeen(lastSeenRaw);

            var ip = device.GetProperty("addresses").EnumerateArray()
                .FirstOrDefault().GetString() ?? string.Empty;

            return new TailscalePeerDto
            {
                Hostname = device.GetProperty("hostname").GetString() ?? string.Empty,
                Ip = ip,
                User = device.TryGetProperty("user", out var u) ? u.GetString() ?? string.Empty : string.Empty,
                Os = device.TryGetProperty("os", out var os) ? os.GetString() ?? string.Empty : string.Empty,
                IsOnline = isOnline,
                LastSeen = lastSeen
            };
        }

        // Convierte ISO timestamp a texto legible: "2h ago", "3d ago"
        private static string FormatLastSeen(string? iso)
        {
            if (string.IsNullOrWhiteSpace(iso)) return "desconocido";
            if (!DateTimeOffset.TryParse(iso, out var dt)) return iso;

            var diff = DateTimeOffset.UtcNow - dt;
            if (diff.TotalMinutes < 1) return "hace un momento";
            if (diff.TotalHours < 1) return $"{(int)diff.TotalMinutes}m ago";
            if (diff.TotalDays < 1) return $"{(int)diff.TotalHours}h ago";
            return $"{(int)diff.TotalDays}d ago";
        }
    }
}
