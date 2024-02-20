using DemoCookieAuthSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoCookieAuthSystem.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicaitonUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
