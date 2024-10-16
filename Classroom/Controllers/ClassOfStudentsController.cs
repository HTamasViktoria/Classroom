using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;

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
            return Ok(_classOfStudentsRepository.GetAll());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("allStudentsWithClasses")]
    public ActionResult<IEnumerable<StudentWithClassResponse>> GetAllStudentsWithClasses()
    {
        try
        {
            var studentsWithClasses = _classOfStudentsRepository.GetAllStudentsWithClasses();
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
        
            if (students == null || !students.Any())
            {
                return Ok(Enumerable.Empty<Student>());
            }

            return Ok(students);
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
        try
        {
            var classes = _classOfStudentsRepository.GetClassesBySubject(subject);
            
            if (!classes.Any())
            {
                return Ok(Enumerable.Empty<ClassOfStudents>());
            }
            
            return Ok(classes);
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
            return Ok(new { message = "Successfully added new grade" });
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }
    
}