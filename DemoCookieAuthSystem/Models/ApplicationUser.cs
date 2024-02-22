using Microsoft.AspNetCore.Identity;

namespace DemoCookieAuthSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
