using DemoCookieAuthSystem.DTOs;
using DemoCookieAuthSystem.Models;
using DemoCookieAuthSystem.Responses;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DemoCookieAuthSystem.Services
{
    public class Account : IAccount
    {
        private readonly UserManager<ApplicaitonUser> _userManager;
        private readonly SignInManager<ApplicaitonUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Account(UserManager<ApplicaitonUser> userManager, SignInManager<ApplicaitonUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto model)
        {
            var findUser = await _userManager.FindByEmailAsync(model.Email);

            if (findUser != null)
                return new ServiceResponse(false, "User already exists");

            var appUser = new ApplicaitonUser()
            {
                Name = model.FullName,
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email
            };

            var createUser = await _userManager.CreateAsync(appUser, model.Password);

            if(createUser.Succeeded)
            {
                var adminRole = await _roleManager.FindByNameAsync("Admin");
                if(adminRole is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _userManager.AddToRoleAsync(appUser!, "Admin");
                }
                else
                {
                    var userRole = await _roleManager.FindByNameAsync("User");
                    if(userRole is null)
                        await _roleManager.CreateAsync(new IdentityRole("User"));

                    await _userManager.AddToRoleAsync(appUser!, "User");
                }
                return new ServiceResponse(true, "User registered");
            }

            return new ServiceResponse(false, "Error while registering user");
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto model)
        {
            var findUser = await _userManager.FindByEmailAsync(model.Email);
            if (findUser is null) 
                return new ServiceResponse(false, "User not found");

            var getRole = (await _userManager.GetRolesAsync(findUser!)).FirstOrDefault();

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, findUser!.Name!),
                new(ClaimTypes.Email, findUser!.Email!),
                new(ClaimTypes.Role, getRole!)
            };

            var result = await _signInManager.CheckPasswordSignInAsync(findUser, model.Password, false);

            if (!result.Succeeded)
                return new ServiceResponse(false, "Error while signing in");

            await _signInManager.SignInWithClaimsAsync(findUser, false, claims);
            return new ServiceResponse(true, "Login Success");
        }
    }
}
