using System.ComponentModel.DataAnnotations;


namespace Classroom.Model.RequestModels
{
    public class StudentRequest
    {
        [Required] 
        public string FirstName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string BirthDate { get; init; }

        [Required]
        public string BirthPlace { get; init; }

        [Required]
        public string StudentNo { get; init; }
    }
}