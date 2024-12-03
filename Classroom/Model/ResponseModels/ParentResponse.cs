using Classroom.Model.DataModels;

public class ParentResponse
{
    public string Id { get; set; }
    public string FamilyName { get; set; }
    public string FirstName { get; set; }
    public string Role { get; set; }
    public string ChildName { get; set; }
    public Student Student { get; set; }
}