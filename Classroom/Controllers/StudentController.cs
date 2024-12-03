using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Classroom.Service;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "Admin,Teacher")]

    public ActionResult<IEnumerable<Student>> GetAll()
    {
        try
        {
            var students = _studentRepository.GetAll();
            if (!students.Any()) 
            {
                return Ok(new List<Student>());
            }

            return Ok(students);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving students.");
            return StatusCode(500, new { Message = $"Internal server error: {e.Message}" });
        }
    }


    [HttpGet("{id}", Name = "GetStudentById")]
    public ActionResult<Student> GetStudentById(string id)
    {
        StringValidationHelper.IsValidId(id);
        try
        {
            var student = _studentRepository.GetStudentById(id);
            
            if (student == null)
            {
                return NotFound(new { Message = $"Student with ID {id} not found." });
            }
            
            return Ok(student);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving student data.");
            return StatusCode(500, new { Message = $"Internal server error: {e.Message}" });
        }
    }

        
    [HttpPost("add")]
    public ActionResult<string> Post([FromBody] StudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }
        try
        {
            _studentRepository.Add(request);
            return Ok(new { message = "Successfully added new student", student = new { request.FirstName, request.FamilyName } });
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(new { message = e.Message });
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error.");
            return StatusCode(500, new { message = "An error occurred while saving to the database." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = $"Internal server error: {e.Message}" });
        }
    }

}