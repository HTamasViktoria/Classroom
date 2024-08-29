using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.DataModels;

public class Student
{
    [Key]
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public DateTime BirthDate { get; init; }
    public string BirthPlace { get; init; }
    public string StudentNo { get; init; }
    public ICollection<Grade> Grades { get; init; } = new List<Grade>();
    
    
    
}