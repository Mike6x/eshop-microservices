using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Users;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public Uri? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool? IsOnline { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }
}