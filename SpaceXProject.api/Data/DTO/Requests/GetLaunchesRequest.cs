namespace SpaceXProject.api.Data.DTO.Requests;

public class GetLaunchesRequest
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string? Search { get; set; }
    public string Type { get; set; } = "all"; // "upcoming", "past", "all"
    public string Sort { get; set; } = "desc"; // "asc", "desc"
}
