using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Oxidized
{
    public class OxidizedVersionDto
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("oid")]
        public string? Oid { get; set; }

        [JsonPropertyName("author")]
        public OxidizedAuthorDto? Author { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        public long? Epoch { get; set; }

        public int Num { get; set; }

        public string? BackupUrl { get; set; }
    }

    public class OxidizedAuthorDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }
    }
}
