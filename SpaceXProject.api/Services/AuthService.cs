using Microsoft.AspNetCore.Identity;
using SpaceXProject.api.Data.DTO.Requests.AuthRequests;
using SpaceXProject.api.Data.DTO.Responses;
using SpaceXProject.api.Data.Models.Authentication;
using SpaceXProject.api.Shared.Base.Error.AuthErrors;
using SpaceXProject.api.Shared.Base.ResultPattern;
using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;

namespace SpaceXProject.api.Services;


public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
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

    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
        try
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
                if (result.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
                {
                    return _resultFactory.Failure<string>(
                        AuthErrors.DuplicateEmailError,
                        ResultStatusEnum.EmailAlreadyExists);
                }

                var errorMessages = result.Errors.Select(e => e.Description).ToArray();
                return _resultFactory.Failure<string>(
                    AuthErrors.RegistrationFailedError(errorMessages),
                    ResultStatusEnum.ValidationFailed);
            }

            return _resultFactory.Success("User registered successfully");
        }
        catch (Exception ex)
        {
            return _resultFactory.Exception<string>(ex, "An unexpected error occured during registration");
        }
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
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
                    ResultStatusEnum.Unauthorized);
            }

            return _resultFactory.Failure<LoginResponse>(
                AuthErrors.UnAuthorizedError,
                ResultStatusEnum.Unauthorized);
        }
        catch (Exception ex)
        {
            return _resultFactory.Exception<LoginResponse>(ex, "An unexpected error occured during registration");
        }
    }

    public async Task<Result> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return _resultFactory.Success();
    }
}

