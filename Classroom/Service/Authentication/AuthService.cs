using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Classroom.Contracts;
using Classroom.Model.DataModels;
using Classroom.Service.Repositories;

namespace Classroom.Service.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
        {
            var user = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = result.Errors.Select(e => new KeyValuePair<string, string>("RegistrationError", e.Description)).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, role);

            return new AuthResult
            {
                Success = true,
                Email = email,
                UserName = username
            };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("LoginError", "Invalid email or password.") }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("LoginError", "Invalid email or password.") }
                };
            }

           

            return new AuthResult
            {
                Success = true,
                Email = user.Email,
                UserName = user.UserName,
                Token = "GeneratedJWTTokenHere"
            };
        }
    }
}
