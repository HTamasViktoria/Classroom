using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Classroom.Service.Repositories;

namespace Classroom.Controllers;

[ApiController]
[Route("api/grades")]

public class GradeController : ControllerBase
{
    private readonly ILogger<GradeController> _logger;
    private readonly IGradeRepository _gradeRepository;

    public GradeController(ILogger<GradeController> logger, IGradeRepository gradeRepository)
    {
        _logger = logger;
        _gradeRepository = gradeRepository;
    }
    
    
    
    [HttpGet(Name = "grades")]
    public ActionResult<IEnumerable<Grade>> GetAll()
    {
        try
        {
            return Ok(_gradeRepository.GetAll());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    [HttpGet("gradeValues")]
    public ActionResult<IEnumerable<string>> GetAllValues()
    {
        try
        {
            var gradeValues = Enum.GetValues(typeof(GradeValues))
                .Cast<GradeValues>()
                .Select(gv => $"{gv.ToString()} = {(int)gv}");

            return Ok(gradeValues);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpPost("add")]
    public ActionResult<string> Post([FromBody] GradeRequest request)
    {
        try
        {
            _gradeRepository.Add(request);
            return Ok(new { message = "Successfully added new grade" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }
    
    
    
    
    [HttpGet("{id}", Name = "GetGradesByStudentId")]
    public ActionResult<IEnumerable<Grade>> GetGradesByStudentId(int id)
    {
        try
        {
            var grades = _gradeRepository.GetByStudentId(id);
        
            if (grades == null)
            {
                return NotFound($"Grades with student ID {id} not found.");
            }
        
            return Ok(grades);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    
    [HttpGet("class-averages/{studentId}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAverageGradesBySubject(int studentId)
    {
        try
        {
            var averages = await _gradeRepository.GetClassAverageGradesBySubjectAsync(studentId);
            return Ok(averages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    
    

}