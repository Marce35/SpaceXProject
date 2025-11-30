using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.api.Data.DTO.Requests.AuthRequests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.Authentication;
using SpaceXProject.api.Services;
using SpaceXProject.api.Shared.Base.ResultPattern;
using SpaceXProject.api.Shared.Constants;
using CookieOptions = Microsoft.AspNetCore.Http.CookieOptions;

namespace SpaceXProject.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public UserController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<UserResponse>>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is { IsSuccess: true, Value: not null })
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(ApplicationConstants.HttpCookieExpirationInMinutes)
                };

                Response.Cookies.Append("X-Access-Token", result.Value.Token, cookieOptions);
                var frontendResult = new Result<UserResponse>(result.Value.User, ResultStatusEnum.Success);

                return Ok(frontendResult);
            }

            return Ok(new Result<UserResponse>(null, result.Status, result.Error));
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();

            Response.Cookies.Delete("X-Access-Token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(result);
        }

        [HttpGet("check-session")]
        public async Task<ActionResult<Result<UserResponse>>> CheckSession()
        {
            var result = await _authService.CheckSessionAsync(User);

            return Ok(result);
        }
    }
}
