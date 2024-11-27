using Microsoft.AspNetCore.Identity;

namespace Classroom.Service.Authentication;


public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}