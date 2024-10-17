using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classroom.Model.DataModels
{
    public class Parent : User
    {
        [Key]
        public int ParentId { get; init; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string ChildName { get; set; }

        [ForeignKey(nameof(StudentId))]
        public int StudentId { get; set; }
    }
}