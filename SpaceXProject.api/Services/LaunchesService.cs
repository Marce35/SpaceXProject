using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.ExternalApiClient.Builders;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using SpaceXProject.api.Shared.Base.ResultPattern;
using SpaceXProject.api.Shared.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace SpaceXProject.api.Services;

public interface ILaunchesService
{
    Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> GetLaunchesAsync(GetLaunchesRequest request);
}

public class LaunchesService : ILaunchesService
{
    private readonly IExternalApiClient _apiClient;
    private readonly IMemoryCache _cache;

    public LaunchesService(IExternalApiClient apiClient, IMemoryCache cache)
    {
        _apiClient = apiClient;
        _cache = cache;
    }

    public async Task<Result<SpaceXPagedResponse<SpaceXLaunch>>> GetLaunchesAsync(GetLaunchesRequest request)
    {
        string cacheKey = $"Launches_P{request.Page}_L{request.Limit}_S{request.Search}_T{request.Type}_O{request.Sort}";

        if (_cache.TryGetValue(cacheKey, out Result<SpaceXPagedResponse<SpaceXLaunch>>? cachedResult))
        {
            return cachedResult!;
        }

        var rocketFields = JsonPropertyHelper.GetPropertyNames<SpaceXRocket>();

        var queryRequest = new SpaceXQueryBuilder<SpaceXLaunch>()
            .WithPagination(request.Page, request.Limit)
            .SortBy(x => x.DateUtc, request.Sort)
            .Search(x => x.Name, request.Search)
            .FilterByStatus(request.Type)
            .Populate(x => x.Rocket, rocketFields)
            .Build();

        var result = await _apiClient.PostSpaceXQueryAsync(queryRequest);

        if (result.IsSuccess)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _cache.Set(cacheKey, result, cacheOptions);
        }

        return result;
    }
}

