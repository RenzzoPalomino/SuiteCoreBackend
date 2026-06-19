using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Models.Entities;

public class NetboxResponse
{
    [JsonPropertyName("results")]
    public List<NetboxRegionResult> Results { get; set; } = new();
}

public class NetboxRegionResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("site_count")]
    public int SiteCount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
