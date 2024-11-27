using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class NotificationRequest
{
   
        public string Type { get; set; }
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string? Subject { get; set; }
        public bool Read { get; set; }
        public bool OfficiallyRead { get; set; }
        public string Description { get; set; }
        public List<string> StudentIds { get; set; }
        public string? OptionalDescription { get; set; }
    
}