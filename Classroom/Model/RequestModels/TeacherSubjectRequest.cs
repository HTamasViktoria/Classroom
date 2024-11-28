using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels.Enums;


namespace Classroom.Model.RequestModels;

public class TeacherSubjectRequest
{
    [Required]
    public string Subject { get; set; }
    [Required]
    public string TeacherId { get; set; }
    [Required]
    public int ClassOfStudentsId { get; set; }
    [Required]
    public string ClassName { get; set; }
    
}