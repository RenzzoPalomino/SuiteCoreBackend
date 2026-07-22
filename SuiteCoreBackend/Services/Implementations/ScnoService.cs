using Microsoft.Extensions.Options;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Net.Http.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class ScnoService : IScnoService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public ScnoService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<HttpResponseMessage> TriggerLifecycleDecommissionRawAsync(string planId, string candidateId, string reason)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/scno/lifecycle/decommission")
            {
                Content = JsonContent.Create(new
                {
                    plan_id = planId,
                    candidate_id = candidateId,
                    reason
                })
            };
            request.Headers.Add("X-SCNO-User", _settings.User);
            request.Headers.Add("X-SCNO-Role", _settings.Role);

            return await _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> TriggerLifecycleOnboardRawAsync(string candidateId) =>
            _httpClient.PostAsJsonAsync("/api/v1/scno/lifecycle/onboard", new { candidate_id = candidateId });
    }
}
