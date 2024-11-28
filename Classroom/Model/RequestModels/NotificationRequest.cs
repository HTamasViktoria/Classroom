using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class NotificationRequest
{
    [Required] public string Type { get; set; }
    [Required] public string TeacherId { get; set; }
    [Required] public string TeacherName { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public DateTime DueDate { get; set; }
    public string? Subject { get; set; }
    [Required] public bool Read { get; set; }
    [Required] public bool OfficiallyRead { get; set; }
    [Required] public string Description { get; set; }
    [Required] public List<string> StudentIds { get; set; }
    public string? OptionalDescription { get; set; }
}