using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Models.Entities;

public class ScnoGrafanaEmbedLinksResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("service")]
    public string Service { get; set; } = string.Empty;

    [JsonPropertyName("embed_links")]
    public ScnoGrafanaEmbedLinksData EmbedLinks { get; set; } = new();
}

public class ScnoGrafanaEmbedLinksData
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public List<ScnoGrafanaEmbedLinkResult> Items { get; set; } = new();
}

public class ScnoGrafanaEmbedLinkResult
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("folder_title")]
    public string FolderTitle { get; set; } = string.Empty;

    [JsonPropertyName("dashboard_url")]
    public string DashboardUrl { get; set; } = string.Empty;

    [JsonPropertyName("embed_url")]
    public string EmbedUrl { get; set; } = string.Empty;
}
