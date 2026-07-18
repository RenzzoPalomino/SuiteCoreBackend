using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Onboarding;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class OnboardingService : IOnboardingService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public OnboardingService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<OnboardingStatusDto> GetStatusAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/onboarding/status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            OnboardingLatestScanDto? latestScan = null;
            if (root.TryGetProperty("latest_scan", out var scanEl) && scanEl.ValueKind == JsonValueKind.Object)
            {
                latestScan = new OnboardingLatestScanDto
                {
                    ScanId            = scanEl.GetStringOrEmpty("scan_id"),
                    Source            = scanEl.GetStringOrEmpty("source"),
                    RequestedBy       = scanEl.GetStringOrEmpty("requested_by"),
                    RequestedRole     = scanEl.GetStringOrEmpty("requested_role"),
                    TotalSeen         = scanEl.TryGetProperty("total_seen",          out var ts) ? ts.GetInt32() : 0,
                    KnownCount        = scanEl.TryGetProperty("known_count",         out var kc) ? kc.GetInt32() : 0,
                    IgnoredCount      = scanEl.TryGetProperty("ignored_count",       out var ic) ? ic.GetInt32() : 0,
                    ManualReviewCount = scanEl.TryGetProperty("manual_review_count", out var mr) ? mr.GetInt32() : 0,
                    EligibleCount     = scanEl.TryGetProperty("eligible_count",      out var ec) ? ec.GetInt32() : 0,
                    PersistedCount    = scanEl.TryGetProperty("persisted_count",     out var pc) ? pc.GetInt32() : 0,
                    CreatedAt         = scanEl.GetStringOrEmpty("created_at")
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

        public Task<OnboardingDiscoveryResultDto> GetLocalDiscoveryAsync() =>
            GetDiscoveryResultAsync("/api/v1/onboarding/discovery/local");

        public Task<OnboardingDiscoveryResultDto> GetTailscaleDiscoveryAsync() =>
            GetDiscoveryResultAsync("/api/v1/onboarding/discovery/tailscale");

        private async Task<OnboardingDiscoveryResultDto> GetDiscoveryResultAsync(string path)
        {
            var json = await _httpClient.GetStringAsync(path);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var rules = root.TryGetProperty("rules", out var rulesEl)
                ? JsonSerializer.Deserialize<Dictionary<string, object?>>(rulesEl.GetRawText()) ?? new()
                : new Dictionary<string, object?>();

            var summary = new OnboardingDiscoverySummaryDto();
            if (root.TryGetProperty("summary", out var summaryEl))
            {
                summary = new OnboardingDiscoverySummaryDto
                {
                    Total        = summaryEl.TryGetProperty("total",         out var t)  ? t.GetInt32()  : 0,
                    Known        = summaryEl.TryGetProperty("known",         out var k)  ? k.GetInt32()  : 0,
                    Ignored      = summaryEl.TryGetProperty("ignored",       out var i)  ? i.GetInt32()  : 0,
                    ManualReview = summaryEl.TryGetProperty("manual_review", out var mr) ? mr.GetInt32() : 0,
                    Eligible     = summaryEl.TryGetProperty("eligible",      out var e)  ? e.GetInt32()  : 0
                };
            }

            var excludedSummary = new Dictionary<string, int>();
            if (root.TryGetProperty("excluded_summary", out var excludedEl))
            {
                foreach (var prop in excludedEl.EnumerateObject())
                {
                    excludedSummary[prop.Name] = prop.Value.ValueKind == JsonValueKind.Number ? prop.Value.GetInt32() : 0;
                }
            }

            var observations = root.TryGetProperty("observations", out var obsEl)
                ? JsonSerializer.Deserialize<List<object>>(obsEl.GetRawText()) ?? new()
                : new List<object>();

            var candidates = new List<OnboardingDiscoveryCandidateDto>();
            if (root.TryGetProperty("candidates", out var candidatesEl))
            {
                foreach (var cand in candidatesEl.EnumerateArray())
                {
                    var metadata = cand.TryGetProperty("metadata", out var metaEl)
                        ? JsonSerializer.Deserialize<Dictionary<string, object?>>(metaEl.GetRawText()) ?? new()
                        : new Dictionary<string, object?>();

                    candidates.Add(new OnboardingDiscoveryCandidateDto
                    {
                        CandidateId     = cand.GetStringOrEmpty("candidate_id"),
                        Source          = cand.GetStringOrEmpty("source"),
                        SourceKey       = cand.GetStringOrEmpty("source_key"),
                        Name            = cand.GetStringOrEmpty("name"),
                        Hostname        = cand.GetStringOrEmpty("hostname"),
                        ManagementIp    = cand.GetStringOrEmpty("management_ip"),
                        OperatingSystem = cand.GetStringOrEmpty("operating_system"),
                        Reason          = cand.GetStringOrEmpty("reason"),
                        Metadata        = metadata
                    });
                }
            }

            return new OnboardingDiscoveryResultDto
            {
                Status          = root.GetStringOrEmpty("status"),
                Source          = root.GetStringOrEmpty("source"),
                Mode            = root.GetStringOrEmpty("mode"),
                CheckedAt       = root.GetStringOrEmpty("checked_at"),
                Rules           = rules,
                Summary         = summary,
                ExcludedSummary = excludedSummary,
                Observations    = observations,
                Candidates      = candidates
            };
        }

        public async Task<OnboardingCandidatesListDto> GetCandidatesAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/onboarding/candidates");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var items = new List<OnboardingCandidateDto>();
            if (root.TryGetProperty("items", out var itemsEl))
            {
                foreach (var item in itemsEl.EnumerateArray())
                {
                    var metadata = item.TryGetProperty("metadata", out var metaEl)
                        ? JsonSerializer.Deserialize<Dictionary<string, object?>>(metaEl.GetRawText()) ?? new()
                        : new Dictionary<string, object?>();

                    items.Add(new OnboardingCandidateDto
                    {
                        CandidateId     = item.GetStringOrEmpty("candidate_id"),
                        Source          = item.GetStringOrEmpty("source"),
                        SourceKey       = item.GetStringOrEmpty("source_key"),
                        Name            = item.GetStringOrEmpty("name"),
                        Hostname        = item.GetStringOrEmpty("hostname"),
                        ManagementIp    = item.GetStringOrEmpty("management_ip"),
                        OperatingSystem = item.GetStringOrEmpty("operating_system"),
                        State           = item.GetStringOrEmpty("state"),
                        Eligibility     = item.GetStringOrEmpty("eligibility"),
                        Reason          = item.GetStringOrEmpty("reason"),
                        FirstSeenAt     = item.GetStringOrEmpty("first_seen_at"),
                        LastSeenAt      = item.GetStringOrEmpty("last_seen_at"),
                        LastOnlineAt    = item.GetStringOrEmpty("last_online_at"),
                        DiscoveryCount  = item.TryGetProperty("discovery_count", out var dc) ? dc.GetInt32() : 0,
                        DiscoveredBy    = item.GetStringOrEmpty("discovered_by"),
                        DiscoveredRole  = item.GetStringOrEmpty("discovered_role"),
                        LastScanId      = item.GetStringOrEmpty("last_scan_id"),
                        CreatedAt       = item.GetStringOrEmpty("created_at"),
                        UpdatedAt       = item.GetStringOrEmpty("updated_at"),
                        Metadata        = metadata
                    });
                }
            }

            return new OnboardingCandidatesListDto
            {
                Status = root.GetStringOrEmpty("status"),
                Count  = root.TryGetProperty("count", out var cnt) ? cnt.GetInt32() : 0,
                Total  = root.TryGetProperty("total", out var tot) ? tot.GetInt32() : 0,
                State  = root.TryGetProperty("state", out var st) && st.ValueKind == JsonValueKind.String ? st.GetString() : null,
                Items  = items
            };
        }

        public async Task<OnboardingPlansListDto> GetPlansAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/onboarding/plans");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var items = new List<OnboardingPlanDto>();
            if (root.TryGetProperty("items", out var itemsEl))
            {
                foreach (var item in itemsEl.EnumerateArray())
                {
                    items.Add(ParsePlan(item));
                }
            }

            return new OnboardingPlansListDto
            {
                Status       = root.GetStringOrEmpty("status"),
                Count        = root.TryGetProperty("count", out var cnt) ? cnt.GetInt32() : 0,
                Total        = root.TryGetProperty("total", out var tot) ? tot.GetInt32() : 0,
                FilterStatus = root.TryGetProperty("filter_status", out var fs) && fs.ValueKind == JsonValueKind.String ? fs.GetString() : null,
                Items        = items
            };
        }

        public async Task<OnboardingPlanDto> GetPlanByIdAsync(string planId)
        {
            var json = await _httpClient.GetStringAsync($"/api/v1/onboarding/plans/{Uri.EscapeDataString(planId)}");
            using var doc = JsonDocument.Parse(json);
            return ParsePlan(doc.RootElement);
        }

        private static OnboardingPlanDto ParsePlan(JsonElement item)
        {
            var planDetail = new OnboardingPlanDetailDto();
            if (item.TryGetProperty("plan", out var planEl))
            {
                var candidate = planEl.TryGetProperty("candidate", out var candEl)
                    ? JsonSerializer.Deserialize<Dictionary<string, object?>>(candEl.GetRawText()) ?? new()
                    : new Dictionary<string, object?>();

                var configuration = planEl.TryGetProperty("configuration", out var configEl)
                    ? JsonSerializer.Deserialize<Dictionary<string, object?>>(configEl.GetRawText()) ?? new()
                    : new Dictionary<string, object?>();

                var steps = new List<OnboardingPlanStepDto>();
                if (planEl.TryGetProperty("steps", out var stepsEl))
                {
                    foreach (var step in stepsEl.EnumerateArray())
                    {
                        steps.Add(new OnboardingPlanStepDto
                        {
                            Action      = step.GetStringOrEmpty("action"),
                            Description = step.GetStringOrEmpty("description"),
                            Order       = step.TryGetProperty("order", out var ord) ? ord.GetInt32() : 0,
                            Rollback    = step.TryGetProperty("rollback", out var rb) && rb.ValueKind == JsonValueKind.String ? rb.GetString() : null,
                            System      = step.GetStringOrEmpty("system")
                        });
                    }
                }

                planDetail = new OnboardingPlanDetailDto
                {
                    AssetKind             = planEl.GetStringOrEmpty("asset_kind"),
                    Candidate             = candidate,
                    Configuration         = configuration,
                    ExecutionBlockReason  = planEl.TryGetProperty("execution_block_reason", out var ebr) && ebr.ValueKind == JsonValueKind.String ? ebr.GetString() : null,
                    ExecutionReady        = planEl.TryGetProperty("execution_ready", out var er) && er.GetBoolean(),
                    RequiresApproval      = planEl.TryGetProperty("requires_approval", out var ra) && ra.GetBoolean(),
                    RiskLevel             = planEl.GetStringOrEmpty("risk_level"),
                    Source                = planEl.GetStringOrEmpty("source"),
                    Steps                 = steps,
                    Type                  = planEl.GetStringOrEmpty("type"),
                    Version               = planEl.GetStringOrEmpty("version")
                };
            }

            var candidateSnapshot = item.TryGetProperty("candidate_snapshot", out var snapEl)
                ? JsonSerializer.Deserialize<Dictionary<string, object?>>(snapEl.GetRawText()) ?? new()
                : new Dictionary<string, object?>();

            return new OnboardingPlanDto
            {
                PlanId         = item.GetStringOrEmpty("plan_id"),
                CandidateId    = item.GetStringOrEmpty("candidate_id"),
                Status         = item.GetStringOrEmpty("status"),
                RiskLevel      = item.GetStringOrEmpty("risk_level"),
                RequestedBy    = item.GetStringOrEmpty("requested_by"),
                RequestedRole  = item.GetStringOrEmpty("requested_role"),
                ApprovedBy     = item.TryGetProperty("approved_by", out var ab) && ab.ValueKind == JsonValueKind.String ? ab.GetString() : null,
                ApprovedRole   = item.TryGetProperty("approved_role", out var ar2) && ar2.ValueKind == JsonValueKind.String ? ar2.GetString() : null,
                ApprovalNote   = item.TryGetProperty("approval_note", out var an) && an.ValueKind == JsonValueKind.String ? an.GetString() : null,
                CreatedAt      = item.GetStringOrEmpty("created_at"),
                ApprovedAt     = item.TryGetProperty("approved_at", out var aa) && aa.ValueKind == JsonValueKind.String ? aa.GetString() : null,
                UpdatedAt      = item.GetStringOrEmpty("updated_at"),
                Plan           = planDetail,
                CandidateSnapshot = candidateSnapshot
            };
        }

        public async Task<OnboardingExecutionReadinessDto> GetExecutionReadinessAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/onboarding/execution/readiness");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Dictionary<string, object?> ParseDict(string propertyName) =>
                root.TryGetProperty(propertyName, out var el)
                    ? JsonSerializer.Deserialize<Dictionary<string, object?>>(el.GetRawText()) ?? new()
                    : new Dictionary<string, object?>();

            List<string> ParseStringList(string propertyName) =>
                root.TryGetProperty(propertyName, out var el)
                    ? el.EnumerateArray().Select(e => e.GetString() ?? string.Empty).ToList()
                    : new List<string>();

            var supportedSteps = root.TryGetProperty("supported_steps", out var ssEl)
                ? ssEl.EnumerateArray().Select(e => e.GetInt32()).ToList()
                : new List<int>();

            var blockedSteps = root.TryGetProperty("blocked_steps", out var bsEl)
                ? JsonSerializer.Deserialize<List<object>>(bsEl.GetRawText()) ?? new()
                : new List<object>();

            var excludedSystems = root.TryGetProperty("excluded_systems", out var esEl)
                ? JsonSerializer.Deserialize<List<object>>(esEl.GetRawText()) ?? new()
                : new List<object>();

            return new OnboardingExecutionReadinessDto
            {
                Success                    = root.TryGetProperty("success", out var suc) && suc.GetBoolean(),
                Service                    = root.GetStringOrEmpty("service"),
                ExecutionScope             = root.GetStringOrEmpty("execution_scope"),
                ExecutionScopes            = ParseStringList("execution_scopes"),
                ExecutorRoles              = ParseStringList("executor_roles"),
                ExecutorInstalled          = root.TryGetProperty("executor_installed", out var ei) && ei.GetBoolean(),
                ExecutionEndpointPublished = root.TryGetProperty("execution_endpoint_published", out var eep) && eep.GetBoolean(),
                ExternalWriteAuthorized    = root.TryGetProperty("external_write_authorized", out var ewa) && ewa.GetBoolean(),
                OverallExecutionReady      = root.TryGetProperty("overall_execution_ready", out var oer) && oer.GetBoolean(),
                Orchestrator               = ParseDict("orchestrator"),
                Connector                  = ParseDict("connector"),
                Connectors                 = ParseDict("connectors"),
                Executors                  = ParseDict("executors"),
                SupportedSteps             = supportedSteps,
                BlockedSteps               = blockedSteps,
                ExcludedSystems            = excludedSystems,
                LegacyOrchestrator         = ParseDict("legacy_orchestrator"),
                ExecutionAvailable         = root.TryGetProperty("execution_available", out var ea) && ea.GetBoolean()
            };
        }

        public Task<HttpResponseMessage> GetExecutionsRawAsync() =>
            _httpClient.GetAsync(
                "/api/v1/onboarding/executions",
                HttpCompletionOption.ResponseHeadersRead);

        public Task<HttpResponseMessage> GetExecutionByIdRawAsync(string executionId) =>
            _httpClient.GetAsync(
                $"/api/v1/onboarding/executions/{Uri.EscapeDataString(executionId)}",
                HttpCompletionOption.ResponseHeadersRead);

        public Task<HttpResponseMessage> GetExecutionStepsRawAsync(string executionId) =>
            _httpClient.GetAsync(
                $"/api/v1/onboarding/executions/{Uri.EscapeDataString(executionId)}/steps",
                HttpCompletionOption.ResponseHeadersRead);

        public Task<HttpResponseMessage> TriggerLocalDiscoveryScanRawAsync() =>
            PostWithScnoHeadersAsync("/api/v1/onboarding/discovery/local/scan");

        public Task<HttpResponseMessage> TriggerTailscaleDiscoveryScanRawAsync() =>
            PostWithScnoHeadersAsync("/api/v1/onboarding/discovery/tailscale/scan");

        public Task<HttpResponseMessage> CreatePlanForCandidateRawAsync(string candidateId) =>
            _httpClient.PostAsync($"/api/v1/onboarding/candidates/{Uri.EscapeDataString(candidateId)}/plan", null);

        public Task<HttpResponseMessage> ExecutePlanRawAsync(string planId) =>
            _httpClient.PostAsync($"/api/v1/onboarding/plans/{Uri.EscapeDataString(planId)}/execute", null);

        private async Task<HttpResponseMessage> PostWithScnoHeadersAsync(string path)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Add("X-SCNO-User", _settings.User);
            request.Headers.Add("X-SCNO-Role", _settings.Role);

            return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        }
    }
}
