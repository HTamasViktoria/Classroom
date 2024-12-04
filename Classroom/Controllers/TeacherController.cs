using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Classroom.Service;

namespace Classroom.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly ITeacherRepository _teacherRepository;
      

        public TeacherController(ILogger<TeacherController> logger, ITeacherRepository teacherRepository)
        {
            _logger = logger;
            _teacherRepository = teacherRepository;
      
        }

        
        [HttpGet("{id}", Name = "GetByTeacherId")]
        public ActionResult<Teacher> GetByTeacherId(string id)
        {
            StringValidationHelper.IsValidId(id);
            try
            {
                var teacher = _teacherRepository.GetTeacherById(id);
                
                if (teacher == null)
                {
                    return NotFound(new { message = $"Teacher with ID {id} not found." });
                }
                
                return Ok(teacher);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving the teacher by ID.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }

        
        
        [HttpGet(Name = "teachers")]
        public ActionResult<IEnumerable<Teacher>> GetAll()
        {
            try
            {
                var teachers = _teacherRepository.GetAll();
                if (!teachers.Any())
                {
                    return Ok(new List<Teacher>());
                }
                return Ok(teachers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
        
        [HttpPost("add")]
        public ActionResult<string> Post([FromBody] TeacherRequest request)
        {
            try
            {
                _teacherRepository.Add(request);
                return Ok("Successfully added new teacher");
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid teacher data.");
                return BadRequest(new { message = e.Message });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error while updating the database.");
                return StatusCode(500, new { message = "An error occurred while saving to the database. Please try again later." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected error occurred.");
                return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
            }
        }

        
        
        
       
    }
}