using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.Data.Models.SpaceXApi.Core;
using SpaceXProject.api.Data.Models.SpaceXApi.Requests;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using SpaceXProject.api.Shared.Base.Error;
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

    public async Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> GetLaunchesAsync(GetLaunchesRequest request)
    {

        var query = new Dictionary<string, object>();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query["name"] = new Dictionary<string, object>
            {
                { "$regex", request.Search },
                { "$options", "i" }
            };
        }

        if (request.Type == "upcoming") query["upcoming"] = true;
        else if (request.Type == "past") query["upcoming"] = false;


        var sort = new { flight_number = request.Sort == "asc" ? "asc" : "desc" };

        var requestBody = new SpaceXQueryRequest
        {
            Query = query,
            Options = new SpaceXQueryOptions
            {
                Page = request.Page,
                Limit = request.Limit,
                Sort = sort,
                Populate = ["rocket"]
            }
        };

        var response = await _httpClient.PostAsJsonAsync("launches/query", requestBody);


        if (!response.IsSuccessStatusCode)
        {
            return _resultFactory.Failure<SpaceXPagedResponse<SpaceXLaunch>>(
                new Error("SpaceX_ApiError", [$"Failed to fetch launches. Status: {response.StatusCode}"]),
                ResultStatusEnum.Failure);
        }

        var pagedResult = await response.Content.ReadFromJsonAsync<SpaceXPagedResponse<SpaceXLaunch>>();

        if (pagedResult == null)
        {
            return _resultFactory.Failure<SpaceXPagedResponse<SpaceXLaunch>>(
                new Error("SpaceX_SerializationError", ["Received empty response from SpaceX"]),
                ResultStatusEnum.Failure);
        }

        return _resultFactory.Success(pagedResult);
    }
}
