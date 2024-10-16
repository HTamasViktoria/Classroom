namespace Classroom.Model.ResponseModels;

public class StudentWithClassResponse
{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public string NameOfClass { get; set; }
}