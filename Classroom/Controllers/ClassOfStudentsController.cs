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

            if (!classes.Any())
            {
                return Ok(new List<ClassOfStudents>());
            }

            return Ok(classes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching classes.");
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("students")]
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
    
    
    [HttpGet("students-of-a-class/{classId}")]
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
            return StatusCode(400, $"Bad request: {knfEx.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    
    [HttpGet("bysubject/{subject}")]
    public ActionResult<IEnumerable<ClassOfStudents>> GetClassesBySubject(string subject)
    {
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
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    
    [HttpPost]
    public IActionResult Post([FromBody] ClassOfStudentsRequest request)
    {
        
        try
        {
            _classOfStudentsRepository.Add(request);
            return CreatedAtAction(nameof(Post), new { id = request.Id }, new { message = "Osztály sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpPost("addStudent")]
    public IActionResult Post([FromBody] AddingStudentToClassRequest request)
    {
        try
        {
            _classOfStudentsRepository.AddStudent(request);
            return StatusCode(200, "Student added to class");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(404, $"Not found: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(400, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal Server error: {ex.Message}");
        }
    }

  
}