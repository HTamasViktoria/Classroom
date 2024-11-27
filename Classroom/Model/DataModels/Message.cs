using Microsoft.AspNetCore.Identity;

namespace Classroom.Model.DataModels;

public class Message
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public User Sender { get; set; }
    public string SenderName { get; set; }
    public User Receiver { get; set; }
    public string ReceiverName { get; set; }
    public string HeadText { get; set; }
    public string Text { get; set; }
    public bool Read { get; set; }
    public bool DeletedBySender { get; set; }
    public bool DeletedByReceiver { get; set; }
}