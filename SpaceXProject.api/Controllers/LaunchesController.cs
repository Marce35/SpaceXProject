using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.api.ExternalApiClient.Interfaces;

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

        [HttpGet("GetLaunches")]
        public async Task<IActionResult> GetLaunches([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var result = await _externalApiClient.GetLaunchesAsync(page, limit);
            return Ok(result);
        }
    }
}
