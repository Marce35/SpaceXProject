using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi;

public class SpaceXRocket
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("cost_per_launch")]
    public long CostPerLaunch { get; set; }

    [JsonPropertyName("success_rate_pct")]
    public int SuccessRatePct { get; set; }

    [JsonPropertyName("height")]
    public Distance Height { get; set; } = new();

    [JsonPropertyName("diameter")]
    public Distance Diameter { get; set; } = new();

    [JsonPropertyName("mass")]
    public Mass Mass { get; set; } = new();

    [JsonPropertyName("flickr_images")]
    public List<string> Images { get; set; } = [];
}
