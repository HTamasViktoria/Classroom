using Classroom.Model.DataModels.Enums;


namespace Classroom.Model.RequestModels;

public class TeacherSubjectRequest
{
    public string Subject { get; set; }
    public int TeacherId { get; set; }
    public int ClassOfStudentsId { get; set; }
    
}