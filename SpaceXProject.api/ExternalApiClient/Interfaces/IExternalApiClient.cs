using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.Data.Models.SpaceXApi.Requests;
using SpaceXProject.api.Shared.Base.ResultPattern;

namespace SpaceXProject.api.ExternalApiClient.Interfaces;

public interface IExternalApiClient
{
    Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> PostSpaceXQueryAsync(SpaceXQueryRequest request);
}
