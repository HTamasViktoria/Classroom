using Classroom.Model.DataModels;

namespace Classroom.Model.RequestModels;

public class GradeRequest
{
    public Teacher Teacher { get; init; }
    public Student Student { get; init; }
    public string Subject { get; init; }
    public int Value { get; init; }
    public DateTime Date { get; init; }
}