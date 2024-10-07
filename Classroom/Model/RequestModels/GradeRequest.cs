using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class GradeRequest
{
    public string TeacherId { get; init; }
    public string StudentId { get; init; }
    public string Subject { get; init; }
    public string ForWhat { get; set; }
    public string Value { get; init; }
    public string Date { get; init; }
}