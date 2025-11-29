using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi;

public class SpaceXCore
{
    [JsonPropertyName("core")]
    public string? CoreId { get; set; }

    [JsonPropertyName("landing_success")]
    public bool? LandingSuccess { get; set; }

    [JsonPropertyName("reused")]
    public bool? Reused { get; set; }
}
