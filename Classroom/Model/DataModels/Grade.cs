using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.DataModels;

public class Grade
{

    [Key]
    public int Id { get; init; }
    public Teacher Teacher { get; set; }
    public Student Student { get; set; }
    public string Subject { get; set; }
    public GradeValues Value { get; set; }
    public DateTime Date { get; set; }

}