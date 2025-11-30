using Microsoft.AspNetCore.Identity;
using SpaceXProject.api.Data.DTO.Requests.AuthRequests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.Authentication;
using SpaceXProject.api.Shared.Base.Error.AuthErrors;
using SpaceXProject.api.Shared.Base.ResultPattern;
using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;
using System.Security.Claims;

namespace SpaceXProject.api.Services;


public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<UserResponse>> CheckSessionAsync(ClaimsPrincipal principal);

    Task<Result> LogoutAsync();
}
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly ITokenService _tokenService;

    private readonly IResultFactory _resultFactory;

    public AuthService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        ITokenService tokenService,
        IResultFactory resultFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;

        _tokenService = tokenService;   

        _resultFactory = resultFactory;
    }

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
            {
                return _resultFactory.Failure(
                    AuthErrors.DuplicateEmailError,
                    ResultStatusEnum.EmailAlreadyExists);
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToArray();
            return _resultFactory.Failure(
                AuthErrors.RegistrationFailedError(errorMessages),
                ResultStatusEnum.ValidationFailed);
        }

        return _resultFactory.Success();
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return _resultFactory.Failure<LoginResponse>(
                AuthErrors.UnAuthorizedError,
                ResultStatusEnum.Unauthorized);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: true, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var token = _tokenService.GenerateToken(user);
            var userResponse = new UserResponse(user.FirstName, user.LastName);
            return _resultFactory.Success(new LoginResponse(token, userResponse));
        }

        if (result.IsLockedOut)
        {
            return _resultFactory.Failure<LoginResponse>(
                AuthErrors.AccountLockedError,
                ResultStatusEnum.Failure);
        }

        return _resultFactory.Failure<LoginResponse>(
            AuthErrors.UnAuthorizedError,
            ResultStatusEnum.Unauthorized);
    }

    public async Task<Result> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return _resultFactory.Success();
    }

    public async Task<Result<UserResponse>> CheckSessionAsync(ClaimsPrincipal principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return _resultFactory.Failure<UserResponse>(
                AuthErrors.UserNotAuthenticated,
                ResultStatusEnum.NotAuthenticated);
        }

        var email = principal.FindFirst(ClaimTypes.Name)?.Value
                    ?? principal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return _resultFactory.Failure<UserResponse>(
                AuthErrors.UserNotAuthenticated,
                ResultStatusEnum.NotAuthenticated);
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return _resultFactory.Failure<UserResponse>(
                AuthErrors.UserNotFound,
                ResultStatusEnum.NotFound);
        }

        return _resultFactory.Success(new UserResponse(user.FirstName, user.LastName));
    }
}

