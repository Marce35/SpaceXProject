using System.ComponentModel.DataAnnotations;
using SpaceXProject.api.Shared.Constants;

namespace SpaceXProject.api.Data.DTO.Requests.AuthRequests;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(ApplicationConstants.MaxIdentityFieldStringLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(ApplicationConstants.MaxIdentityFieldStringLength)]
    public string LastName { get; set; } = string.Empty;


    [Required]
    [MinLength(ApplicationConstants.MinPasswordLength)]
    [MaxLength(ApplicationConstants.MaxPasswordLength)]
    [RegularExpression(ApplicationConstants.PasswordContainsNumericAndSpecialCharRegex, ErrorMessage = "The password must contain at least one numeric value and at least one special character")]
    public required string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "The password and the confirm password need to be the same")]
    public required string ConfirmPassword { get; set; }
}
