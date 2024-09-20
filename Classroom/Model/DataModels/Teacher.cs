using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Classroom.Model.DataModels;

public class Teacher
{
    [Key]
    public int Id { get; init; }
    public string FamilyName { get; set; }
    public string FirstName { get; set; }
    
}