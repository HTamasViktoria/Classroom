using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classroom.Model.DataModels
{
    public class Parent : User
    {
     
       
        public string ChildName { get; set; }

        [ForeignKey(nameof(StudentId))]
        public string StudentId { get; set; }
    }
}