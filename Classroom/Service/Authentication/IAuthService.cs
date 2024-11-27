namespace Classroom.Service.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role, string firstName,
        string familyName, DateTime? birthDate, string? birthPlace, string? studentNo, string? childName, string? studentId);
    Task<AuthResult> LoginAsync(string email, string password);
}