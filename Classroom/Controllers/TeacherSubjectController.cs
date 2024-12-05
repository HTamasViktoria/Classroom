using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;

namespace Classroom.Controllers;

[ApiController]
[Route("api/teachersubjects")]

public class TeacherSubjectController : ControllerBase
{
    private readonly ILogger<TeacherSubjectController> _logger;
    private readonly ITeacherSubjectRepository _teacherSubjectRepository;
      

    public TeacherSubjectController(ILogger<TeacherSubjectController> logger, ITeacherSubjectRepository teacherSubjectRepository)
    {
        _logger = logger;
        _teacherSubjectRepository = teacherSubjectRepository;
    }


    [HttpGet("byteacher/{teacherId}")]
    public ActionResult<IEnumerable<TeacherSubject>> GetSubjectsByTeacherId(string teacherId)
    {
        try
        {
            StringValidationHelper.IsValidId(teacherId);
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
            return StatusCode(400, $"Bad request:{e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving subjects for the teacher.");
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }



    [HttpPost]
    public ActionResult<object> Post([FromBody] TeacherSubjectRequest request)
    {
        try
        {
            _teacherSubjectRepository.Add(request);
      
            return CreatedAtAction(nameof(GetSubjectsByTeacherId), new { teacherId = request.TeacherId }, "Successfully added new teacherSubject" );

        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(400, $"Bad request: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    [HttpGet("studentsof/{teacherSubjectId}")]
    public async Task<IActionResult> GetStudentsByTeacherSubjectId(int teacherSubjectId)
    {
        try
        {
            var classOfStudents = await _teacherSubjectRepository.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId);

            if (classOfStudents == null)
            {
                return StatusCode(400, $"Bad request: No ClassOfStudents found for TeacherSubject ID {teacherSubjectId}.");
            }

            var students = classOfStudents.Students;
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving students for TeacherSubjectId.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }




  
}