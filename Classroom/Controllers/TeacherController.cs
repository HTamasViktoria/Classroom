using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Classroom.Service.Repositories;

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
                    return NotFound($"Teacher with ID {id} not found.");
                }
                return Ok(teacher);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
        
        [HttpGet(Name = "teachers")]
        public ActionResult<IEnumerable<Teacher>> GetAll()
        {
            try
            {
                return Ok(_teacherRepository.GetAll());
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
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
        
        
       
    }
}