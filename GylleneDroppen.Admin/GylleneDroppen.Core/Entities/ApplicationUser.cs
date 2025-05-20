using Microsoft.AspNetCore.Identity;

namespace GylleneDroppen.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}