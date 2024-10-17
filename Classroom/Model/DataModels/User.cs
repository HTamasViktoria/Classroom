using Microsoft.AspNetCore.Identity;

namespace Classroom.Model.DataModels;

public abstract class User : IdentityUser
{
    public string Name { get; set; }
    public string FamilyName { get; set; }


    
}