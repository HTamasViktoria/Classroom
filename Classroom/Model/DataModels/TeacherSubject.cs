using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Model.DataModels
{
    public class TeacherSubject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }
        
        [ForeignKey(nameof(TeacherId))]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        
        [ForeignKey(nameof(ClassOfStudentsId))]
        public int ClassOfStudentsId { get; set; }
        public ClassOfStudents ClassOfStudents { get; set; }
        public string ClassName { get; set; }
    }
}