using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class StudentRequest
{
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string BirthDate { get; init; }
    public string BirthPlace { get; init; }
    public string StudentNo { get; init; }
    


}