namespace Classroom.Service.Authentication
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<KeyValuePair<string, string>> ErrorMessages { get; set; }
    }
}