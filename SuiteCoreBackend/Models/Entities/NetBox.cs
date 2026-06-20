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

public class NetboxIpResponse
{
    [JsonPropertyName("results")]
    public List<NetboxIpAddressResult> Results { get; set; } = new();
}

public class NetboxIpAddressResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("dns_name")]
    public string DnsName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxStatusResult
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;
}
