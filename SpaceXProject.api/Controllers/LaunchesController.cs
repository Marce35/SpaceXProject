using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.api.Data.DTO.Requests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.SpaceXApi;
using SpaceXProject.api.Services;
using SpaceXProject.api.Shared.Base.ResultPattern;

namespace SpaceXProject.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LaunchesController : ControllerBase
    {

        private readonly ILaunchesService _launchesService;

        public LaunchesController(ILaunchesService launchesService)
        {
            _launchesService = launchesService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<SpaceXPagedResponse<SpaceXLaunch>>>> GetLaunches([FromQuery] GetLaunchesRequest request)
        {
            var result = await _launchesService.GetLaunchesAsync(request);
            return Ok(result);
        }

    }
}
