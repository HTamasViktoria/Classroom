using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class ClassOfStudentsRequest
{
    public int Id { get; set; }
    public string Grade { get; set; }
    public string Section { get; set; }
    
    public ICollection<Student> Students { get; set; } = new List<Student>();
}