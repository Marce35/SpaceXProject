using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi;

public class SpaceXLaunch
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("flight_number")]
    public int FlightNumber { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("date_utc")]
    public DateTime DateUtc { get; set; }

    [JsonPropertyName("success")]
    public bool? Success { get; set; }

    [JsonPropertyName("details")]
    public string? Details { get; set; }

    [JsonPropertyName("upcoming")]
    public bool Upcoming { get; set; }

    [JsonPropertyName("rocket")]
    public SpaceXRocket? Rocket { get; set; }

    [JsonPropertyName("cores")]
    public List<SpaceXCore> Cores { get; set; } = new();
}

