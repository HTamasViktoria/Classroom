using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.ResponseModels;

public class LatestGradeResponse
{
    public int Id { get; set; }
    public string TeacherId { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string Subject { get; set; }
    public string ForWhat { get; set; }
    public bool Read { get; set; }
    public GradeValues Value { get; set; }
    public DateTime Date { get; set; }
}