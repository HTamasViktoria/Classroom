using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Classroom.Service.Repositories;

namespace Classroom.Controllers;

[ApiController]
[Route("api/students")]


public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly IStudentRepository _studentRepository;

    public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
    {
        _logger = logger;
        _studentRepository = studentRepository;
    }
    
    
    [HttpGet(Name = "students")]
    public ActionResult<IEnumerable<Student>> GetAll()
    {
        try
        {
            return Ok(_studentRepository.GetAll());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpGet("{id}", Name = "GetStudentById")]
    public ActionResult<Student> GetStudentById(int id)
    {
        try
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(student);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
        
        
    [HttpPost("add")]
    public ActionResult<string> Post([FromBody] StudentRequest request)
    {
        try
        {
            _studentRepository.Add(request);
            return Ok("Successfully added new student");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}