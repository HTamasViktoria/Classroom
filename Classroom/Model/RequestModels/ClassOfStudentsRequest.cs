using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class ClassOfStudentsRequest
{
    public int Id { get; set; }
    [Required]
    public string Grade { get; set; }
    [Required]
    public string Section { get; set; }
    [Required]
    public ICollection<Student> Students { get; set; } = new List<Student>();
}