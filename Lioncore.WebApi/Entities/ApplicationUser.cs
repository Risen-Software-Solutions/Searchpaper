using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lioncore.WebApi.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FullName { get; set; } = string.Empty;
}
