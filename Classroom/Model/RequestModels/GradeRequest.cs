using System.ComponentModel.DataAnnotations;
namespace Classroom.Model.RequestModels;

public class GradeRequest
{
    [Required]
    public string TeacherId { get; init; }
    [Required]
    public string StudentId { get; init; }
    [Required]
    public string Subject { get; init; }
    [Required]
    public string ForWhat { get; set; }
    [Required]
    public bool Read { get; set; }
    public string Value { get; init; }
    [Required]
    public string Date { get; init; }
}