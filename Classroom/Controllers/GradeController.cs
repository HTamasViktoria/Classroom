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
    
    
    
    [HttpGet(Name = "grades")]//ezt használom egyáltalán valahol?
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
    
    
    

    [HttpPut("edit/{id}")] 
    public ActionResult<string> Put([FromBody] GradeRequest request, int id)
    {
        
        try
        {
            _gradeRepository.Edit(request,id);
            return Ok(new { message = "Successfully updated grade" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }
    
    
    [HttpDelete("delete/{id}")] 
    public ActionResult<string> Delete(int id)
    {
        
        try
        {
            _gradeRepository.Delete(id);
            return Ok(new { message = "Successfully deleted grade" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }
    
    
    
    
    [HttpGet("{id}", Name = "GetGradesByStudentId")]
    public ActionResult<IEnumerable<Grade>> GetGradesByStudentId(string id)
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

    
    
    
    [HttpGet("{classId}/{subject}", Name = "GetGradesByClassBySubject")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByClassBySubject(int classId, string subject)
    {
        try
        {
            var grades = await _gradeRepository.GetGradesByClassBySubject(classId, subject);
        
            if (grades == null || !grades.Any())
            {
                return NotFound($"No grades found for class ID {classId} and subject {subject}.");
            }
        
            return Ok(grades);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("getGradesByClass/{classId}", Name = "GetGradesByClass")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByClass(int classId)
    {
        try
        {
            var grades = await _gradeRepository.GetGradesByClass(classId);

            if (grades == null || !grades.Any())
            {
                return NotFound($"No grades found for class ID {classId}.");
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
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesByStudentId(string studentId)
    {
        try
        {
            var averages = await _gradeRepository.GetClassAveragesByStudentId(studentId);
            return Ok(averages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    [HttpGet("class-average/{subject}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesBySubject(string subject)
    {
        try
        {
            var averages = await _gradeRepository.GetClassAveragesBySubject(subject);
            if (averages == null || !averages.Any())
            {
                return NotFound(new { message = "No classes found for the specified subject." });
            }

            return Ok(averages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    
    

}