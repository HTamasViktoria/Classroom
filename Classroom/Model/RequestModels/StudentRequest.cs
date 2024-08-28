using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class StudentRequest
{
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public DateTime BirthDate { get; init; }
    public string BirthPlace { get; init; }
    public string StudentNo { get; init; }
    public ICollection<GradeValues> Grades { get; init; } = new List<GradeValues>();


}