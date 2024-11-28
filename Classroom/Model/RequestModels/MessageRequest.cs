using System.ComponentModel.DataAnnotations;
using Classroom.Model.DataModels;

namespace Classroom.Model.RequestModels;

public class MessageRequest
{
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string FromId { get; set; }
    [Required]
    public List<string> ReceiverIds { get; set; }
    public string HeadText { get; set; }
    [Required]
    public string Text { get; set; }
}