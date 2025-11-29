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

            return MapResultToActionResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.IsSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(ApplicationConstants.HttpCookieExpirationInMinutes)
                };

                Response.Cookies.Append("X-Access-Token", result.Value!, cookieOptions);

                return Ok(new { Message = "Login Successful" });

            }
            return MapResultToActionResult(result);
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

            return MapResultToActionResult(result);
        }

        [HttpGet("check-session")]
        public async Task<IActionResult> CheckSession()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? User.Identity.Name;

                if (email is not null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        return Ok(new UserResponse(user.FirstName, user.LastName));
                    }
                }
            }

            return Unauthorized(new { isAuthenticated = false });
        }


        #region ResultMapper

        private IActionResult MapResultToActionResult<T>(Result<T> result)
        {
            return result.Status switch
            {
                ResultStatusEnum.Success => Ok(result.Value),
                ResultStatusEnum.EmailAlreadyExists => Conflict(result.Error),
                ResultStatusEnum.Unauthorized => Unauthorized(result.Error),
                ResultStatusEnum.ValidationFailed => BadRequest(result.Error),
                ResultStatusEnum.NotFound => NotFound(result.Error),
                ResultStatusEnum.Exception => StatusCode(500, result.Error),
                _ => BadRequest(result.Error != null ? result.Error : new { Message = "An unspecified error occured." })
            };
        }

        private IActionResult MapResultToActionResult(Result result)
        {
            return result.Status switch
            {
                ResultStatusEnum.Success => Ok(new { Message = "Operation successful" }),
                ResultStatusEnum.Unauthorized => Unauthorized(result.Error),
                ResultStatusEnum.ValidationFailed => BadRequest(result.Error),
                ResultStatusEnum.NotFound => NotFound(result.Error),
                ResultStatusEnum.Exception => StatusCode(500, result.Error),
                _ => BadRequest(result.Error)
            };
        }

        #endregion
    }
}
