using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Classroom.Model.DataModels
{
    public class ClassOfStudents
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        
        public ICollection<Student> Students { get; set; } = new List<Student>();
       
    }
}