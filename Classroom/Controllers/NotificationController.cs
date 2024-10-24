using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;

namespace Classroom.Controllers;

[ApiController]
[Route("api/notifications")]

public class NotificationController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;

    public NotificationController(ILogger<StudentController> logger, INotificationRepository notificationRepository, INotificationService notificationService)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
    }
    
    [HttpGet(Name = "notifications")]
    public ActionResult<IEnumerable<NotificationBase>> GetAll()
    {
        try
        {
            return Ok(_notificationRepository.GetAll());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    
    [HttpGet("byStudentId/{id}", Name = "GetByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetByStudentId(string id)
    {
        try
        {
            var notifications = _notificationRepository.GetByStudentId(id);
        
            if (!notifications.Any())
            {
                return Ok(new List<NotificationBase>());
            }
        
            return Ok(notifications);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("lastsByStudentId/{id}", Name = "GetLastsByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetLastsByStudentId(string id)
    {
        try
        {
            var notifications = _notificationRepository.GetLastsByStudentId(id);
        
            if (!notifications.Any())
            {
                return Ok(new List<NotificationBase>());
            }
        
            return Ok(notifications);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    
    
    
    [HttpPost("add")]
    public IActionResult Post([FromBody] NotificationRequest request)
    {
        try
        {
            _notificationService.PostToDb(request);
            return Ok(new { message = "Értesítés sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás értesítési adat.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
    
    
    [HttpPost("setToRead/{id}")]
    public IActionResult SetToRead(int id)
    {
        try
        {
            _notificationRepository.SetToRead(id);
            return Ok(new { message = "Értesítés sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás értesítési adat.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }

    
    
    [HttpGet("homeworks", Name = "GetHomeworks")]
    public ActionResult<IEnumerable<NotificationBase>> GetHomeworks()
    {
        try
        {
            var homeworks = _notificationRepository.GetHomeworks();
            return Ok(homeworks);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("exams", Name = "GetExams")]
    public ActionResult<IEnumerable<NotificationBase>> GetExams()
    {
        try
        {
            var exams = _notificationRepository.GetExams();
            return Ok(exams);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("others", Name = "GetOthers")]
    public ActionResult<IEnumerable<NotificationBase>> GetOthers()
    {
        try
        {
            var others = _notificationRepository.GetOthers();
            return Ok(others);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("missingEquipments", Name = "GetMissingEquipments")]
    public ActionResult<IEnumerable<NotificationBase>> GetMissingEquipments()
    {
        try
        {
            var missingEquipments = _notificationRepository.GetMissingEquipments();
            return Ok(missingEquipments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    [HttpDelete("delete/{id}")]
    public ActionResult<object> Delete(int id)
    {
        try
        {
            _notificationRepository.Delete(id);
            return Ok(new { message = "Successful delete" });
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { error = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { error = "An error occurred while processing your request." });
        }
    }



    
}