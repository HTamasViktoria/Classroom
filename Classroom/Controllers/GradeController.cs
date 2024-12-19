using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;
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
            var grades = _gradeRepository.GetAll();
            if (!grades.Any())
            {
                return Ok(new List<Grade>());
            }
            return Ok(grades);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    
    [HttpGet("gradevalues")]
    public ActionResult<IEnumerable<string>> GetAllValues()
    {
        try
        {
            var gradeValues = Enum.GetValues(typeof(GradeValues))
                .Cast<GradeValues>()
                .OrderBy(gv => gv)
                .Select(gv => $"{gv.ToString()} = {(int)gv}")
                .ToList();

            if (!gradeValues.Any())
            {
                return StatusCode(404, "No gradevalues found");
            }

            return Ok(gradeValues);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
   

    [HttpPost]
    public IActionResult Post([FromBody] GradeRequest request)
    {
        try
        {
            _gradeRepository.Add(request);
            return CreatedAtAction(nameof(GetAllValues), new { }, new { message = "Értesítés sikeresen elmentve az adatbázisba." });
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

    
    
    

    [HttpGet("bysubject/{subject}/bystudent/{studentId}")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesBySubjectByStudent(string subject, string studentId)
    {
        try
        {
            var grades = await _gradeRepository.GetGradesBySubjectByStudent(subject, studentId);

            if (!grades.Any())
            {
                return Ok(new List<Grade>());
            }

            return Ok(grades);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put([FromBody] GradeRequest request, int id)
    {
        try
        {
            _gradeRepository.Edit(request, id);
        
            return Ok(new { message = "Successfully updated grade" });
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, knfEx.Message);
            return StatusCode(404, $"Grade with Id {id} not found.");
        }
        catch (ArgumentException argEx)
        {
            _logger.LogError(argEx, argEx.Message);
            return StatusCode(400, $"Bad request: {argEx.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _gradeRepository.Delete(id);
            return StatusCode(200,"Osztályzat sikeresen törölve.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(404, $"Grade with Id {id} not found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    
    
    [HttpGet("{id}", Name = "GetGradesByStudentId")]
    public ActionResult<IEnumerable<Grade>> GetGradesByStudentId(string id)
    {
        try
        {
            var grades = _gradeRepository.GetByStudentId(id);
        
            if (!grades.Any())
            {
                return Ok(new List<Grade>());
            }

            return Ok(grades);
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


    
    
    [HttpGet("teacherslast/{id}", Name = "GetTeachersLastGrade")]
    public async Task<ActionResult<LatestGradeResponse>> GetTeachersLastGrade(string id)
    {
       
        try
        {
            var grade = await _gradeRepository.GetTeachersLastGradeAsync(id);

            if (grade == null)
            {
                return Ok(new LatestGradeResponse());
            }

            return Ok(grade);
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


    
    
    [HttpGet("newgradesnum/{id}", Name = "GetNewGradesNumber")]
    public ActionResult<int> GetNewGradesNumber(string id)
    {
        try
        {
            var newGradesNumber = _gradeRepository.GetNewGradesNumber(id);

            return Ok(newGradesNumber);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid student ID: {0}", ex.Message);
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching new grades: {0}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    
    
    
    
    [HttpGet("newgrades/{id}", Name = "GetNewGradesByStudentId")]
    public ActionResult<IEnumerable<Grade>> GetNewGradesByStudentId(string id)
    {
        try
        {
            var newGrades = _gradeRepository.GetNewGradesByStudentId(id);
            if (!newGrades.Any())
            {
                return Ok(new List<Grade>());
            }
            return Ok(newGrades);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
    
    [HttpGet("byclass/{classId}/bysubject/{subject}")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByClassBySubject(int classId, string subject)
    {
        try
        {
            var grades = await _gradeRepository.GetGradesByClassBySubject(classId, subject);

            if (!grades.Any())
            {
                return Ok(new List<Grade>());
            }

            return Ok(grades);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    
    [HttpGet("byclass/{classId}", Name = "GetGradesByClass")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByClass(int classId)
    {
        try
        {
            var grades = await _gradeRepository.GetGradesByClass(classId);

            if (!grades.Any())
            {
                return Ok(new List<Grade>());
            }

            return Ok(grades);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    
    [HttpGet("class-averages/bystudent/{studentId}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesByStudentId(string studentId)
    {
        try
        {
            var averages = await _gradeRepository.GetClassAveragesByStudentId(studentId);
            return Ok(averages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    
    [HttpGet("class-averages/bysubject/{subject}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesBySubject(string subject)
    {
        try
        {
            var averages = await _gradeRepository.GetClassAveragesBySubject(subject);
            return Ok(averages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpPost("officiallyread/{id}")]
    public ActionResult SetToOfficiallyRead(int id)
    {
        try
        {
            _gradeRepository.SetToOfficiallyRead(id);
            return StatusCode(200, "Grade marked as officially read.");
        }
        catch (ArgumentException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                return StatusCode(404, $"{ex.Message}");
            }
            
            return BadRequest($"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    
    

}