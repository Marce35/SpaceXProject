using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi;

public class Distance
{
    [JsonPropertyName("meters")]
    public double? Meters { get; set; }

    [JsonPropertyName("feet")]
    public double? Feet { get; set; }
}
