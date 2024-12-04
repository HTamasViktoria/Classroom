using System.ComponentModel.DataAnnotations;

namespace Classroom.Model.RequestModels;

public class AddingStudentToClassRequest
{
    [Required]
    public string StudentId { get; set; }
    [Required]
    public int ClassId { get; set; }
}