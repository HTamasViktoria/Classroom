using System.ComponentModel.DataAnnotations;

namespace Classroom.Model.RequestModels;

public class TeacherRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string FamilyName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
   

}