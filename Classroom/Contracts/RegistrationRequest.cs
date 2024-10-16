using System.ComponentModel.DataAnnotations;

namespace Classroom.Contracts;

public record RegistrationRequest(
    [Required] string Email,
    [Required] string Username,
    [Required] string Password);