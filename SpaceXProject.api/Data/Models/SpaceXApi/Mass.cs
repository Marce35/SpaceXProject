using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi;

public class Mass
{
    [JsonPropertyName("kg")]
    public double? Kg { get; set; }

    [JsonPropertyName("lb")]
    public double? Lb { get; set; }
}
