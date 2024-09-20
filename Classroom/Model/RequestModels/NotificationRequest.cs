using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.RequestModels;

public class NotificationRequest
{
   
        public string Type { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime Date { get; set; }
        public string? Subject { get; set; }
        public bool Read { get; set; }
        public string Description { get; set; }
        public List<int> StudentIds { get; set; }
        public string? OptionalDescription { get; set; }
    
}