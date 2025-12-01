using SpaceXProject.api.Data.Enums;

namespace SpaceXProject.api.Data.DTO.Requests;

public class GetLaunchesRequest
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string? Search { get; set; }

    public LaunchStatusFilter Type { get; set; } = LaunchStatusFilter.All;

    public SortDirection Sort { get; set; } = SortDirection.Desc;
}
