using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var subjects = _teacherSubjectRepository.GetSubjectsByTeacherId(teacherId);

            if (subjects == null || !subjects.Any())
            {
      
                return Ok(new List<TeacherSubject>());
            }

            return Ok(subjects);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpPost("add")]
    public ActionResult<object> Post([FromBody] TeacherSubjectRequest request)
    {
        try
        {
            _teacherSubjectRepository.Add(request);
            return Ok(new { Message = "Successfully added new teacherSubject" });
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
        var classOfStudents = await _teacherSubjectRepository.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId);

        if (classOfStudents == null)
        {
            return NotFound();
        }

        
        var students = classOfStudents.Students;
        Console.WriteLine("Diákok lista:");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Név: {student.FirstName}{student.FamilyName}"); }

        return Ok(students);
    }


  
}