using Microsoft.AspNetCore.Identity;

namespace DemoCookieAuthSystem.Models
{
    public class ApplicaitonUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
