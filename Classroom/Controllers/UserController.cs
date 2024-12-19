using Microsoft.AspNetCore.Mvc;
using Classroom.Service.Repositories;
using Classroom.Model.DataModels;
using Classroom.Model.ResponseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Classroom.Service;

namespace Classroom.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet("allteachers")]
        public ActionResult<IEnumerable<Teacher>> GetAllTeachers()
        {
            try
            {
                var teachers = _userRepository.GetAllTeachers();
                if (!teachers.Any())
                {
                    return Ok(new List<Teacher>());
                }
                return Ok(teachers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving all teachers.");
                return StatusCode(500, new { message = "Internal server error. Please try again later." });
            }
        }
        
        
        
        [HttpGet("allparents")]
        public ActionResult<IEnumerable<Parent>> GetAllParents()
        {
            try
            {
                var parents = _userRepository.GetAllParents();
                if (!parents.Any())
                {
                    return Ok(new List<Parent>());
                }
                
                return Ok(parents);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving all parents.");
                return StatusCode(500, new { message = "Internal server error. Please try again later." });
            }
        }

        
        [HttpGet("teachers/{teacherId}")]
        public ActionResult<Teacher> GetTeacherById(string teacherId)
        {
            
            try
            {
                var teacher = _userRepository.GetTeacherById(teacherId);
                return Ok(teacher);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument error");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected error occurred while retrieving the teacher.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }


        [HttpGet("parents/{parentId}")]
        public ActionResult<Parent> GetParentById(string parentId)
        {
            try
            {
                var parent = _userRepository.GetParentById(parentId);
                return Ok(parent);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument exception occurred while retrieving parent.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected error occurred while retrieving the parent.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }
        
        
        
        [HttpPost("teachers")]
        public ActionResult<string> AddTeacher([FromBody] Teacher teacher)
        {
            try
            {
                _userRepository.AddTeacher(teacher);
                return CreatedAtAction(nameof(GetTeacherById), new { teacherId = teacher.Id }, teacher);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Failed to add a teacher.");
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding teacher.");
                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while adding a teacher.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }

        
        
        
        
        

        [HttpPost("parents")]
        public ActionResult<string> AddParent([FromBody] Parent parent)
        {
            
            try
            {
                _userRepository.AddParent(parent);
                return CreatedAtAction(nameof(GetParentById), new { parentId = parent.Id }, parent);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Failed to add a parent.");
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding parent.");
                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while adding a parent.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }

        
        
        [HttpGet("teacherreceivers")]
        public ActionResult<IEnumerable<IdentityUser>> GetTeacherReceivers()
        {
            try
            {
                var users = _userRepository.GetTeachersAsReceivers();
        
                if (users.Any())
                {
                    return Ok(users);
                }
    
                return Ok(new List<ReceiverResponse>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt: {ex.Message}");
            }
        }

        
        
        
        [HttpGet("parentreceivers")]
        public ActionResult<IEnumerable<IdentityUser>> GetParentReceivers()
        {
            try
            {
                var users = _userRepository.GetParentsAsReceivers();
        
                if (users.Any())
                {
                    return Ok(users);
                }
    
                return Ok(new List<ReceiverResponse>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt: {ex.Message}");
            }
        }

    }
}
