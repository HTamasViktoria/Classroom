using System.ComponentModel.DataAnnotations.Schema;
using Classroom.Model.DataModels;

public class NotificationResponse
{
    public int Id { get; set; }
    public string TeacherId { get; set; }
    public string TeacherName { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    [ForeignKey("Student")]
    public string StudentId { get; set; }
    public Student Student { get; set; }

    public string ParentName {get;set; }
    public string StudentName { get; set; }

    public string Description { get; set; }
    public string? SubjectName { get; set; }
    public bool Read { get; set; }
    public bool OfficiallyRead { get; set; }
    public string? OptionalDescription { get; set; }
}