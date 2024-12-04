using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service;

namespace Classroom.Controllers;


[ApiController]
[Route("api/classes")]

public class ClassOfStudentsController : ControllerBase
{
    private readonly ILogger<ClassOfStudentsController> _logger;
    private readonly IClassOfStudentsRepository _classOfStudentsRepository;

    public ClassOfStudentsController(ILogger<ClassOfStudentsController> logger, IClassOfStudentsRepository classOfStudentsRepository)
    {
        _logger = logger;
        _classOfStudentsRepository = classOfStudentsRepository;
    }

    [HttpGet(Name = "classes")]
    public ActionResult<IEnumerable<ClassOfStudents>> GetAll()
    {
        try
        {
            var classes = _classOfStudentsRepository.GetAll();
        
            if ( !classes.Any())
            {
                return Ok(new List<ClassOfStudents>());
            }

            return Ok(classes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching classes.");
            return StatusCode(500, new { message = "An error occurred, please try again later." });
        }
    }

    
    
    [HttpGet("allStudentsWithClasses")]
    public ActionResult<IEnumerable<StudentWithClassResponse>> GetAllStudentsWithClasses()
    {
        try
        {
            var studentsWithClasses = _classOfStudentsRepository.GetAllStudentsWithClasses();
            if (!studentsWithClasses.Any())
            {
                return Ok(new List<StudentWithClassResponse>());
            }
            return Ok(studentsWithClasses);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("getStudents/{classId}")]
    public ActionResult<IEnumerable<Student>> GetStudents(int classId)
    {
        
        try
        {
            var students = _classOfStudentsRepository.GetStudents(classId);
            
            if (!students.Any())
            {
                return Ok(new List<Student>());
            }
            
            return Ok(students);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, knfEx.Message);
            return NotFound(knfEx.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    
    [HttpGet("getClassesBySubject/{subject}")]
    public ActionResult<IEnumerable<ClassOfStudents>> GetClassesBySubject(string subject)
    {
        StringValidationHelper.IsValidId(subject);
        try
        {
            var classes = _classOfStudentsRepository.GetClassesBySubject(subject);

            if (!classes.Any())
            {
                return Ok(new List<ClassOfStudents>());
            }
            
            return Ok(classes);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    
    [HttpPost("add")]
    public ActionResult<string> Post([FromBody] ClassOfStudentsRequest request)
    {
        
        try
        {
            _classOfStudentsRepository.Add(request);
            return Ok(new { message = "Successfully added new class" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }


    
    [HttpPost("addStudent")]
    public ActionResult<string> Post([FromBody] AddingStudentToClassRequest request)
    {
        
        try
        {
            _classOfStudentsRepository.AddStudent(request);
            return Ok(new { message = "Successfully added new student" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        }
    }

    
}