using Microsoft.AspNetCore.Mvc;
using Classroom.Contracts;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service;
using Classroom.Service.Authentication;


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
        
        
        
        
        [HttpPost("sign-up/student")]
        public async Task<ActionResult<RegistrationResponse>> Register(StudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(
                request.Email,
                request.Username,
                request.Password,
                role: "Student",
                request.FirstName,
                request.FamilyName,
                DateTime.Parse(request.BirthDate),
                request.BirthPlace,
                request.StudentNo,
                childName: null,
                studentId: null
            );

            if (!result.Success)
            {
   
                var errorMessages = result.ErrorMessages.ToDictionary(e => e.Key, e => e.Value);
        
                return _authenticationService.HandleErrors(errorMessages);
            }

            var response = new RegistrationResponse(result.Email, result.UserName);
            return CreatedAtAction(nameof(Register), new { email = result.Email }, response);
        }
        
        
        
        [HttpPost("sign-up/teacher")]
        public async Task<ActionResult<RegistrationResponse>> Register(TeacherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(
                request.Email,
                request.Username,
                request.Password,
                role:"Teacher",
                request.FirstName,
                request.FamilyName,
                null,
                null,
                null,
                childName: null,
                studentId:null
            );

            if (!result.Success)
            {
   
                var errorMessages = result.ErrorMessages.ToDictionary(e => e.Key, e => e.Value);
        
                return _authenticationService.HandleErrors(errorMessages);
            }
            var response = new RegistrationResponse(result.Email, result.UserName);

            return CreatedAtAction(nameof(Register), new { email = result.Email }, response);
        }
        
        

        [HttpPost("sign-up/parent")]
        public async Task<ActionResult<RegistrationResponse>> Register(ParentRequest request)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }
            var validationErrors = _userService.ValidateParentRegistration(request.StudentId, request.ChildName);
            if (validationErrors.Any())
            {
            
                foreach (var error in validationErrors)
                {
                    ModelState.AddModelError("ValidationError", error);
                }
                return BadRequest(ModelState);
            }
        

            var result = await _authenticationService.RegisterAsync(
                request.Email,
                request.Username,
                request.Password,
                role: "Parent",
                firstName: request.FirstName,
                familyName: request.FamilyName,
                birthDate: null,
                birthPlace: null,
                studentNo: null,
                childName: request.ChildName,
                studentId: request.StudentId
            );

            if (!result.Success)
            {
   
                var errorMessages = result.ErrorMessages.ToDictionary(e => e.Key, e => e.Value);
        
                return _authenticationService.HandleErrors(errorMessages);
            }
            var response = new RegistrationResponse(result.Email, result.UserName);

            return CreatedAtAction(nameof(Register), new { email = result.Email }, response);
        }

  
        [HttpPost("sign-in")]
        public async Task<ActionResult<UserResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data.", errors = ModelState });
            }

            var result = await _authenticationService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }

            if (result.Email == _configuration["AdminUser:Email"])
            {
                var admin = new Admin
                {
                    UserName = result.Email,
                    Email = result.Email,
                    Role = "Admin"
                };

                return Ok(admin);
            }

            var userResp = _userService.GetByEmail(result.Email);

            if (userResp == null)
            {
                return NotFound(new { message = "User not found." });
            }

            userResp.Role = result.Role;

            HttpContext.Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(30)
            });

            return Ok(new { message = "Login successful", user = userResp });
        }

        
      
        
        [HttpPost("sign-out")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            HttpContext.Session.Clear();

            return NoContent();
        }

      
    }
}
