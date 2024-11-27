namespace Classroom.Model.ResponseModels
{
    public class RegistrationResponse
    {
        public string Email { get; }
        public string UserName { get; }

        public RegistrationResponse(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }
    }
}