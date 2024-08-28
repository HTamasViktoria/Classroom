using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classroom.Model.DataModels;

public class Teacher
{
    [Key]
    public int Id { get; init; }
    public string FamilyName { get; set; }
    public string FirstName { get; set; }
   
    
}