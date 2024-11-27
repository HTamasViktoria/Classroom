using Microsoft.AspNetCore.Identity;

namespace Classroom.Model.DataModels;

public abstract class User : IdentityUser
{
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public string Role { get; set; }


    
}