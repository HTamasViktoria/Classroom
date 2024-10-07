using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.DataModels;

public class Grade
{

    [Key]
    public int Id { get; init; }
    [ForeignKey("TeacherId")]
    public int TeacherId { get; set; }
    
    [ForeignKey("StudentId")]
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public string ForWhat { get; set; }
    public GradeValues Value { get; set; }
    public DateTime Date { get; set; }

}

