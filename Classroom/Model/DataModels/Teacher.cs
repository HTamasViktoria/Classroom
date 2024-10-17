using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Classroom.Model.DataModels;

public class Teacher : User
{
    [Key]
    public int Id { get; init; }
    public string FamilyName { get; set; }
    public string FirstName { get; set; }
    
}