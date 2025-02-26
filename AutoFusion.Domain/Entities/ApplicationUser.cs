using Microsoft.AspNetCore.Identity;

namespace AutoFusion.Domain.Entities;

public enum AccessLevel
{
    Administrator,
    Salesperson,
    Manager
}

public class ApplicationUser : IdentityUser
{
    public AccessLevel AccessLevel { get; set; }
}
