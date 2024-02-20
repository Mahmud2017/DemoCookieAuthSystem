using DemoCookieAuthSystem.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace DemoCookieAuthSystem.Authentication
{
    internal sealed class IdentityRevalidateAuthStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory, IOptions<IdentityOptions> options) : RevalidatingServerAuthenticationStateProvider(loggerFactory)
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IOptions<IdentityOptions> _options = options;

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(20);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicaitonUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicaitonUser> userManager, ClaimsPrincipal principal)
        {
            var user = userManager.GetUserAsync(principal);

            if(user is null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(_options.Value.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(await user);
                return principalStamp == userStamp;
            }
        }
    }
}
