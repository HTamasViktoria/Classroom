using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.DataModels
{
    public class NotificationBase
    {
        [Key]
        public int Id { get; init; }

        [ForeignKey("TeacherId")]
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }
    
        public string Description { get; set; }
        public Subjects? Subject { get; set; }
        public string? SubjectName { get; set; }
        public bool Read { get; set; }
        public bool OfficiallyRead { get; set; }
        public string? OptionalDescription { get; set; }
    }
}