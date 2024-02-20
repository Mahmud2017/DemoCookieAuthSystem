using DemoCookieAuthSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DemoCookieAuthSystem.Authentication
{
    internal static class SignoutEndpoint
    {
        internal static IEndpointConventionBuilder MapSignoutEndpoint(this IEndpointRouteBuilder endpoint)
        {
            ArgumentNullException.ThrowIfNull(endpoint);

            var accountGroup = endpoint.MapGroup("/Account");
            accountGroup.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicaitonUser> signInManager) => 
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect("/");
            });

            return accountGroup;
        }
    }
}
