using Microsoft.AspNetCore.Mvc;
using Classroom.Model.DataModels.Enums;
using System.Linq;

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
            
            var subjects = Enum.GetValues(typeof(Subjects))
                .Cast<Subjects>()
                .Select(s => s.ToString())
                .ToList();
            
            return Ok(subjects);
        }
    }
}