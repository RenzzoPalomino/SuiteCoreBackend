using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Backup
{
    public class OxidizedDeviceDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        [JsonPropertyName("group")]
        public string? Group { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("last")]
        public OxidizedLastDto? Last { get; set; }

        [JsonPropertyName("mtime")]
        public string? LastChanged { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class OxidizedLastDto
    {
        [JsonPropertyName("start")]
        public string? Start { get; set; }

        [JsonPropertyName("end")]
        public string? End { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("time")]
        public double? Time { get; set; }
    }
}
