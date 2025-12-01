using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.Data.Models.SpaceXApi.Core;
using SpaceXProject.api.Data.Models.SpaceXApi.Requests;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using SpaceXProject.api.Shared.Base.Error;
using SpaceXProject.api.Shared.Base.Error.SpaceXApiErrors;
using SpaceXProject.api.Shared.Base.ResultPattern;
using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;

namespace SpaceXProject.api.ExternalApiClient;
public class ExternalApiClient : IExternalApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IResultFactory _resultFactory;

    public ExternalApiClient(HttpClient httpClient, IResultFactory resultFactory)
    {
        _httpClient = httpClient;
        _resultFactory = resultFactory;
    }

    public async Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> PostSpaceXQueryAsync(SpaceXQueryRequest request)
    {

        // No try/catch needed here, Middleware handles generic HTTP exceptions
        var response = await _httpClient.PostAsJsonAsync("launches/query", request);

        if (!response.IsSuccessStatusCode)
        {
            return _resultFactory.Failure<SpaceXPagedResponse<SpaceXLaunch>>(
                    SpaceXApiErrors.ApiError(response.StatusCode.ToString()),
                    ResultStatusEnum.Failure);
        }

        var pagedResult = await response.Content.ReadFromJsonAsync<SpaceXPagedResponse<SpaceXLaunch>>();

        if (pagedResult is null)
        {
            return _resultFactory.Failure<SpaceXPagedResponse<SpaceXLaunch>>(
                SpaceXApiErrors.SerializationError(),
                ResultStatusEnum.Failure);
        }

        return _resultFactory.Success(pagedResult);
    }
}
