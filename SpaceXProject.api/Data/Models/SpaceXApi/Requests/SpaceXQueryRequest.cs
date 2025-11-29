using System.Text.Json.Serialization;
using SpaceXProject.api.Data.Models.SpaceXApi.Core;

namespace SpaceXProject.api.Data.Models.SpaceXApi.Requests;

public class SpaceXQueryRequest
{
    [JsonPropertyName("query")]
    public object Query { get; set; } = new { }; 

    [JsonPropertyName("options")]
    public SpaceXQueryOptions Options { get; set; } = new();
}
