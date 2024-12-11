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
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("{id}", Name = "GetStudentById")]
    public ActionResult<Student> GetStudentById(string id)
    {
       
        try
        {
         
            var student = _studentRepository.GetStudentById(id);
            
            if (student == null)
            {
                return StatusCode(400, $"Bad request:Student with the given id not found");
            }
            
            return Ok(student);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving student data.");
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

        
    [HttpPost]
    public ActionResult<string> Post([FromBody] StudentRequest request)
    {
        try
        {
            _studentRepository.Add(request);
            
            return CreatedAtAction(nameof(GetStudentById), new { id = request.Username }, "Successfully added new student");
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(400, $"Bad request: {e.Message}");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error.");
            return StatusCode(500, $"Internal server error: {dbEx.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

}