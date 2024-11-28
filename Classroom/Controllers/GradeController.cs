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
    
    [HttpGet("gradeValues")]
    public ActionResult<IEnumerable<string>> GetAllValues()
    {
        try
        {
            var gradeValues = Enum.GetValues(typeof(GradeValues))
                .Cast<GradeValues>()
                .Select(gv => $"{gv.ToString()} = {(int)gv}")
                .ToList();

            if (!gradeValues.Any())
            {
                return NotFound(new { error = "No grade values found." });
            }

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
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }
        try
        {
            _gradeRepository.Add(request);

            return Ok(new { message = "Successfully added new grade" });
        }
        catch (ArgumentException ex)
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

    [HttpGet("getGradesBySubjectByStudent/{subject}/{studentId}")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesBySubjectByStudent(string subject, string studentId)
    {
        StringValidationHelper.IsValidId(studentId);
        StringValidationHelper.IsValidId(subject);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        }
    }


    [HttpPut("edit/{id}")]
    public ActionResult<string> Put([FromBody] GradeRequest request, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }
        try
        {
            _gradeRepository.Edit(request, id);
            return Ok(new { message = "Successfully updated grade" });
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, knfEx.Message);
            return NotFound(knfEx.Message);
        }
        catch (ArgumentException argEx)
        {
            _logger.LogError(argEx, argEx.Message);
            return BadRequest(new { error = argEx.Message });
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
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(new { error = ex.Message });
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
        StringValidationHelper.IsValidId(id);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }


    
    
    [HttpGet("teachersLast/{id}", Name = "GetTeachersLastGrade")]
    public async Task<ActionResult<LatestGradeResponse>> GetTeachersLastGrade(string id)
    {
        StringValidationHelper.IsValidId(id);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = $"Internal server error: {e.Message}" });
        }
    }


    
    
    [HttpGet("getNewGradesNumber/{id}", Name = "GetNewGradesNumber")]
    public ActionResult<int> GetNewGradesNumber(string id)
    {
        StringValidationHelper.IsValidId(id);
        try
        {
            var newGradesNumber = _gradeRepository.GetNewGradesNumber(id);
            return Ok(newGradesNumber);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { error = "Hiba történt a kérés feldolgozása során." });
        }
    }

    
    
    [HttpGet("newGrades/{id}", Name = "GetNewGradesByStudentId")]
    public ActionResult<IEnumerable<Grade>> GetNewGradesByStudentId(string id)
    {
        
        StringValidationHelper.IsValidId(id);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { error = "Hiba történt a kérés feldolgozása során." });
        }
    }
    
    
    
    [HttpGet("getGradesByClassBySubject/{classId}/{subject}")]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByClassBySubject(int classId, string subject)
    {
        
        StringValidationHelper.IsValidId(subject);
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { error = "Internal server error: " + ex.Message });
        }
    }

    
    
    [HttpGet("getGradesByClass/{classId}", Name = "GetGradesByClass")]
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
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = "Internal server error: " + e.Message });
        }
    }


    
    [HttpGet("class-averages/byStudent/{studentId}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesByStudentId(string studentId)
    {
        StringValidationHelper.IsValidId(studentId);
        try
        {
            var averages = await _gradeRepository.GetClassAveragesByStudentId(studentId);
            return Ok(averages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }

    
    
    [HttpGet("class-averages/bySubject/{subject}")]
    public async Task<ActionResult<Dictionary<string, double>>> GetClassAveragesBySubject(string subject)
    {
        
        StringValidationHelper.IsValidId(subject);
        try
        {
            var averages = await _gradeRepository.GetClassAveragesBySubject(subject);
            return Ok(averages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }

    
    [HttpPost("setToOfficiallyRead/{id}")]
    public ActionResult SetToOfficiallyRead(int id)
    {
        try
        {
            _gradeRepository.SetToOfficiallyRead(id);
            return Ok(new { message = "Grade marked as officially read." });
        }
        catch (ArgumentException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
            
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, new { message = "An error occurred while processing the request." });
        }
    }


    
    

}