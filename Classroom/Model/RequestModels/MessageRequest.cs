using Classroom.Model.DataModels;

namespace Classroom.Model.RequestModels;

public class MessageRequest
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string FromId { get; set; }
    public List<string> ReceiverIds { get; set; }
    public string HeadText { get; set; }
    public string Text { get; set; }
}