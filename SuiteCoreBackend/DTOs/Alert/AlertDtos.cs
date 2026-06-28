using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Alert
{
    public class GrafanaWebhookDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("alerts")]
        public List<GrafanaAlertItem> Alerts { get; set; } = new();
    }

    public class GrafanaAlertItem
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("labels")]
        public Dictionary<string, string> Labels { get; set; } = new();

        [JsonPropertyName("annotations")]
        public Dictionary<string, string> Annotations { get; set; } = new();
    }

    public class LibreNmsWebhookDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public int State { get; set; }

        [JsonPropertyName("severity")]
        public string Severity { get; set; } = string.Empty;

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
    }
}
