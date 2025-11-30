using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using SpaceXProject.api.Shared.Base.ResultPattern;

namespace SpaceXProject.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LaunchesController : ControllerBase
    {

        private readonly IExternalApiClient _externalApiClient;

        public LaunchesController(IExternalApiClient externalApiClient)
        {
            _externalApiClient = externalApiClient;
        }

        [HttpGet]
        // [FromQuery] maps the URL params (?page=1&limit=10) to the C# Object
        public async Task<ActionResult<Result<SpaceXPagedResponse<SpaceXLaunch>>>> GetLaunches([FromQuery] GetLaunchesRequest request)
        {
            var result = await _externalApiClient.GetLaunchesAsync(request);
            return Ok(result);
        }

    }
}
