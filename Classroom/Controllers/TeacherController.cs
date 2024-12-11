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
            try
            {
                var teacher = _teacherRepository.GetTeacherById(id);

                if (teacher == null)
                {
                    return StatusCode(404, $"Bad request: Teacher with this id not found.");
                }

                return Ok(teacher);
            }
            catch (ArgumentException e)
            {
                _logger.LogWarning(e, e.Message);
                return StatusCode(400, $"Bad request:{e.Message}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving the teacher by ID.");
                return StatusCode(500, $"Internal server error: {e.Message}");
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
        
        
        [HttpPost]
        public ActionResult<string> Post([FromBody] TeacherRequest request)
        {
            try
            {
                _teacherRepository.Add(request);
                return CreatedAtAction(nameof(GetByTeacherId), new { id = request.Username }, "Successfully added new teacher");
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Invalid teacher data.");
                return StatusCode(400, $"Bad request:{e.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error while updating the database.");
                return StatusCode(500, $"Internal server error: {dbEx.Message}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected error occurred.");
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }


        
        
        
        
        
        
       
    }
}