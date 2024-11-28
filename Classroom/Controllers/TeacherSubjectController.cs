using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;

namespace Classroom.Controllers;

[ApiController]
[Route("api/teacherSubjects")]

public class TeacherSubjectController : ControllerBase
{
    private readonly ILogger<TeacherSubjectController> _logger;
    private readonly ITeacherSubjectRepository _teacherSubjectRepository;
      

    public TeacherSubjectController(ILogger<TeacherSubjectController> logger, ITeacherSubjectRepository teacherSubjectRepository)
    {
        _logger = logger;
        _teacherSubjectRepository = teacherSubjectRepository;
    }
    
    
    [HttpGet("getByTeacherId/{teacherId}")]
    public ActionResult<IEnumerable<TeacherSubject>> GetSubjectsByTeacherId(string teacherId)
    {
        StringValidationHelper.IsValidId(teacherId);
        try
        {
            var subjects = _teacherSubjectRepository.GetSubjectsByTeacherId(teacherId);
            if (!subjects.Any())
            {
                return Ok(new List<TeacherSubject>());
            }
            return Ok(subjects);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving subjects for the teacher.");
            return StatusCode(500, new { message = "An error occurred while retrieving the subjects. Please try again later." });
        }
    }



    [HttpPost("add")]
    public ActionResult<object> Post([FromBody] TeacherSubjectRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }
        try
        {
            _teacherSubjectRepository.Add(request);
            return CreatedAtAction("GetSubjectsByTeacherId", new { teacherId = request.TeacherId }, new { Message = "Successfully added new teacherSubject" });
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { Error = $"Internal server error: {e.Message}" });
        }
    }

    
    
    [HttpGet("getStudentsByTeacherSubjectId/{teacherSubjectId}")]
    public async Task<IActionResult> GetStudentsByTeacherSubjectId(int teacherSubjectId)
    {
        try
        {
            var classOfStudents = await _teacherSubjectRepository.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId);

            if (classOfStudents == null)
            {
                return NotFound(new { message = $"No ClassOfStudents found for TeacherSubject ID {teacherSubjectId}." });
            }
            
            var students = classOfStudents.Students;
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving students for TeacherSubjectId.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }



  
}