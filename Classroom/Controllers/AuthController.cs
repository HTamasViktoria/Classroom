using Microsoft.AspNetCore.Mvc;
using Classroom.Contracts;
using Classroom.Model.ResponseModels;
using Classroom.Service;
using Classroom.Service.Authentication;
using Classroom.Service.Repositories;

namespace Classroom.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authenticationService, IConfiguration configuration, IUserService userService)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password, (_configuration.GetSection("AppRoles:UserRoleName").Value));

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
        }

        
        [HttpPost("sign-in")]
        public async Task<ActionResult<UserResponse>> Authenticate([FromBody] AuthRequest request)
        {
            Console.WriteLine($"Received AuthRequest: Email={request.Email}, Password={request.Password}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            var userResp = _userService.GetByEmail(result.Email);

            HttpContext.Response.Cookies.Append("access_token", result.Token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMinutes(30) });

            return Ok(userResp);
        }

        
        [HttpPost("sign-out")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            return Ok();
        }

        private void AddErrors(AuthResult result)
        {
            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }
    }
}
