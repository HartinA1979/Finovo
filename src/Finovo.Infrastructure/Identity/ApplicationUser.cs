using Microsoft.AspNetCore.Identity;

namespace Finovo.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Tenant-scoped role: Owner | Admin | User | SuperAdmin
    /// </summary>
    public string Role { get; set; } = "User";

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    /// <summary>
    /// UI language preference: "de" | "en"
    /// </summary>
    public string Language { get; set; } = "de";

    public bool IsActive { get; set; } = true;
}
