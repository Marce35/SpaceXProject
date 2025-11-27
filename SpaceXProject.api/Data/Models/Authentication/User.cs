using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SpaceXProject.api.Shared.Constants;

namespace SpaceXProject.api.Data.Models.Authentication;

public class User : IdentityUser
{
    [Required]
    [ProtectedPersonalData]
    public override string Email { get; set; } = default!;


    [Required]
    [MaxLength(ApplicationConstants.MaxIdentityFieldStringLength)]
    [ProtectedPersonalData]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(ApplicationConstants.MaxIdentityFieldStringLength)]
    [ProtectedPersonalData]
    public required string LastName { get; set; }
}