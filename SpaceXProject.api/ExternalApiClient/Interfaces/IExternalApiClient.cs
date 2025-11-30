using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.Shared.Base.ResultPattern;

namespace SpaceXProject.api.ExternalApiClient.Interfaces;

public interface IExternalApiClient
{
    Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> GetLaunchesAsync(GetLaunchesRequest request);

}
