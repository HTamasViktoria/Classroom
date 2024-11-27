using Microsoft.AspNetCore.Identity;
using Classroom.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Classroom.Service.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }



        public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role,
            string firstName, string familyName, DateTime? birthDate, string? birthPlace, string? studentNo,
            string? childName, string? studentId)
        {
            if (role == "Student")
            {
                var student = new Student
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    FamilyName = familyName,
                    BirthDate = birthDate ?? DateTime.Now,
                    BirthPlace = birthPlace,
                    StudentNo = studentNo
                };

                var result = await _userManager.CreateAsync(student, password);

                if (!result.Succeeded)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessages = result.Errors.Select(e =>
                            new KeyValuePair<string, string>("RegistrationError", e.Description)).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(student, role);
            }
            else if (role == "Parent")
            {
                var parent = new Parent
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    FamilyName = familyName,
                    ChildName = childName,
                    StudentId = studentId,
                };

                var result = await _userManager.CreateAsync(parent, password);

                if (!result.Succeeded)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessages = result.Errors.Select(e =>
                            new KeyValuePair<string, string>("RegistrationError", e.Description)).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(parent, role);
            }
            else if (role == "Teacher")
            {
                var teacher = new Teacher
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    FamilyName = familyName,
                  
                };

                var result = await _userManager.CreateAsync(teacher, password);

                if (!result.Succeeded)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessages = result.Errors.Select(e =>
                            new KeyValuePair<string, string>("RegistrationError", e.Description)).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(teacher, role);
            }

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
                    ErrorMessages = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("LoginError", "Invalid email or password.")
                    }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("LoginError", "Invalid email or password.")
                    }
                };
            }

            
            var role = await _userManager.GetRolesAsync(user);

            return new AuthResult
            {
                Success = true,
                Email = user.Email,
                UserName = user.UserName,
                Token = "GeneratedJWTTokenHere",
                Role = role.FirstOrDefault()
            };
        }
        
        
        public ActionResult HandleErrors(Dictionary<string, string> errorMessages)
        {
            
            if (errorMessages.Any(error => error.Value.Contains("email already taken")))
            {
                return new ConflictObjectResult(new { message = "Email is already taken." });
            }

            if (errorMessages.Any(error => error.Value.Contains("username already taken")))
            {
                return new ConflictObjectResult(new { message = "Username is already taken." });
            }

        
            var modelState = new ModelStateDictionary();
            foreach (var error in errorMessages)
            {
                modelState.AddModelError(error.Key, error.Value);
            }

            return new BadRequestObjectResult(modelState);
        }
    }
}
