using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class AuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly IConfiguration _configuration;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager; 
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        await AddRoles();
        await CreateAdminUser(); 
    }

    private async Task AddRoles()
    {
        await CreateRole(_configuration["AppRoles:StudentRoleName"]);
        await CreateRole(_configuration["AppRoles:ParentRoleName"]);
        await CreateRole(_configuration["AppRoles:TeacherRoleName"]);
        await CreateRole(_configuration["AppRoles:AdminRoleName"]); 
    }

    private async Task CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            Console.WriteLine($"Role created: {roleName}");
        }
        else
        {
            Console.WriteLine($"Role already exists: {roleName}");
        }
    }

    private async Task CreateAdminUser()
    {
        var adminEmail = _configuration["AdminUser:Email"];
        var adminPassword = _configuration["AdminUser:Password"];
        var adminUsername = _configuration["AdminUser:Username"];

        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, _configuration["AppRoles:AdminRoleName"]);
                Console.WriteLine($"Admin user created: {adminEmail} with username: {adminUsername}");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Admin user already exists: {adminEmail}");
        }
    }
}
