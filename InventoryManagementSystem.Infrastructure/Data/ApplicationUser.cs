using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Infrastructure.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName => $"{FirstName} {LastName}".Trim();
}
