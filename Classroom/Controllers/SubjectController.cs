using Microsoft.AspNetCore.Mvc;
using Classroom.Model.DataModels.Enums;

namespace Classroom.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ILogger<SubjectController> _logger;

        public SubjectController(ILogger<SubjectController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var subjects = Enum.GetValues(typeof(Subjects))
                    .Cast<Subjects>()
                    .Select(s => s.ToString())
                    .ToList();

                if (subjects == null || !subjects.Any())
                {
                    return NotFound(new { message = "No subjects found." });
                }

                return Ok(subjects);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subjects.");
                return BadRequest(new { message = "Invalid enum or internal error while fetching subjects." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                return StatusCode(500, new { message = "Internal server error." });
            }
        }
    }
}