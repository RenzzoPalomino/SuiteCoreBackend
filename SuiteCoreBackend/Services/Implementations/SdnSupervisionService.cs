using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.SdnSupervision;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class SdnSupervisionService : ISdnSupervisionService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public SdnSupervisionService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<SdnSupervisionTopologyDto> GetTopologyAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/topology");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var ports = root.TryGetProperty("ports", out var portsEl)
                ? portsEl.EnumerateArray().Select(p => p.GetString() ?? string.Empty).ToList()
                : new List<string>();

            return new SdnSupervisionTopologyDto
            {
                Status     = root.GetStringOrEmpty("status"),
                Bridge     = root.GetStringOrEmpty("bridge"),
                Controller = root.GetStringOrEmpty("controller"),
                OvsVersion = root.GetStringOrEmpty("ovs_version"),
                Ports      = ports
            };
        }

        public async Task<SdnSupervisionFlowsDto> GetFlowsAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/flows");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var flows = new List<SdnFlowEntryDto>();
            if (root.TryGetProperty("flows", out var flowsEl))
            {
                foreach (var flow in flowsEl.EnumerateArray())
                {
                    flows.Add(new SdnFlowEntryDto
                    {
                        Table    = flow.TryGetProperty("table", out var t) ? t.GetInt32() : 0,
                        Priority = flow.TryGetProperty("priority", out var p) ? p.GetInt32() : 0,
                        Actions  = flow.GetStringOrEmpty("actions"),
                        Raw      = flow.GetStringOrEmpty("raw")
                    });
                }
            }

            return new SdnSupervisionFlowsDto
            {
                Status    = root.GetStringOrEmpty("status"),
                FlowCount = root.TryGetProperty("flow_count", out var fc) ? fc.GetInt32() : 0,
                Flows     = flows
            };
        }

        public async Task<OnboardingStatusDto> GetOnboardingStatusAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/onboarding/status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            OnboardingLatestScanDto? latestScan = null;
            if (root.TryGetProperty("latest_scan", out var scanEl) && scanEl.ValueKind == JsonValueKind.Object)
            {
                latestScan = new OnboardingLatestScanDto
                {
                    ScanId             = scanEl.GetStringOrEmpty("scan_id"),
                    Source             = scanEl.GetStringOrEmpty("source"),
                    RequestedBy        = scanEl.GetStringOrEmpty("requested_by"),
                    RequestedRole      = scanEl.GetStringOrEmpty("requested_role"),
                    TotalSeen          = scanEl.TryGetProperty("total_seen",          out var ts) ? ts.GetInt32() : 0,
                    KnownCount         = scanEl.TryGetProperty("known_count",         out var kc) ? kc.GetInt32() : 0,
                    IgnoredCount       = scanEl.TryGetProperty("ignored_count",       out var ic) ? ic.GetInt32() : 0,
                    ManualReviewCount  = scanEl.TryGetProperty("manual_review_count", out var mr) ? mr.GetInt32() : 0,
                    EligibleCount      = scanEl.TryGetProperty("eligible_count",      out var ec) ? ec.GetInt32() : 0,
                    PersistedCount     = scanEl.TryGetProperty("persisted_count",     out var pc) ? pc.GetInt32() : 0,
                    CreatedAt          = scanEl.GetStringOrEmpty("created_at")
                };
            }

            return new OnboardingStatusDto
            {
                Status         = root.GetStringOrEmpty("status"),
                Integrity      = root.GetStringOrEmpty("integrity"),
                CandidateCount = root.TryGetProperty("candidate_count", out var cc) ? cc.GetInt32() : 0,
                ScanCount      = root.TryGetProperty("scan_count",      out var sc) ? sc.GetInt32() : 0,
                LatestScan     = latestScan
            };
        }

        public async Task<DecommissionManifestDto> GetDecommissionManifestAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/decommission/manifest");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new DecommissionManifestDto
            {
                Status            = root.GetStringOrEmpty("status"),
                Manifest          = root.GetStringOrEmpty("manifest"),
                Sha256            = root.GetStringOrEmpty("sha256"),
                RouterosPreserved = root.TryGetProperty("routeros_preserved", out var rp) && rp.GetBoolean()
            };
        }
    }
}
