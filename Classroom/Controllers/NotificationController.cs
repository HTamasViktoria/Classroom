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
    
    
    [HttpPost("add")]
    public IActionResult Post([FromBody] NotificationRequest request)
    {
        Console.WriteLine("----------------------bejövő kérés--------------------------------------");
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

}