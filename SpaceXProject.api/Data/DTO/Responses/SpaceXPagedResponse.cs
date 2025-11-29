using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.DTO.Responses;
public class SpaceXPagedResponse<T>
{
    [JsonPropertyName("docs")]
    public List<T> Docs { get; set; } = new();

    [JsonPropertyName("totalDocs")]
    public int TotalDocs { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPrevPage")]
    public bool HasPrevPage { get; set; }
}
