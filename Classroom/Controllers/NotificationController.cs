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
    
    
    
    [HttpGet("byStudent/byParent/{studentId}/{parentId}", Name = "GetByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetByStudentId(string studentId, string parentId)
    {
        try
        {
            var notifications = _notificationRepository.GetByStudentId(studentId, parentId);
        
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
    
    
    [HttpGet("ofTeacher/{id}", Name = "GetTeachersNotifications")]
    public ActionResult<IEnumerable<NotificationBase>> GetByTeacherId(string id)
    {
        try
        {
            var notifications = _notificationRepository.GetByTeacherId(id);
        
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
    
    
    [HttpGet("lastsByStudentId/{studentId}/{parentId}", Name = "GetLastsByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetLastsByStudentId(string studentId, string parentId)
    {
        try
        {
            var notifications = _notificationRepository.GetLastsByStudentId(studentId, parentId);
        
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


    [HttpGet("getNewNotifsNumber/{studentId}/{parentId}", Name = "GetNewNotifsNumber")]
    public ActionResult<int> GetNewNotifsNumber(string studentId, string parentId)
    {
        try
        {
            var newNotifsNumber = _notificationRepository.GetNewNotifsNumber(studentId, parentId);
            return Ok(newNotifsNumber);
        }
        catch (Exception ex)
        {
          
            return StatusCode(500, "Hiba történt a kérés feldolgozása során.");
        }
    }

    
    [HttpGet("newNotifsByStudentId/{studentId}/{parentId}", Name = "GetNewNotifsByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetNewNotifsByStudentId(string studentId, string parentId)
    {
        try
        {
            var notifications = _notificationRepository.GetNewNotifsByStudentId(studentId, parentId);
        
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

    
    
    [HttpGet("lastsByTeacherId/{id}", Name = "GetLastsByTeacherId")]
    public ActionResult<IEnumerable<NotificationBase>> GetLastsByTeacherId(string id)
    {
        try
        {
            var notifications = _notificationRepository.GetLastsByTeacherId(id);
        
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
    
    [HttpGet("newestByTeacherId/{id}", Name = "GetNewestByTeacherId")]
    public ActionResult<NotificationBase?> GetNewestByTeacherId(string id)
    {
        try
        {
            var notification = _notificationRepository.GetNewestByTeacherId(id);
            if (notification == null)
            {
                return Ok(new NotificationBase());
            }

            return Ok(notification);
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
            _logger.LogError(ex, "Hibás értesítési adat történt a 'Post' metódusban.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során a 'Post' metódusban.");
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
    
    
    
    [HttpPost("setToOfficiallyRead/{id}")]
         public IActionResult SetToOfficiallyRead(int id)
         {
             try
             {
                 _notificationRepository.SetToOfficiallyRead(id);
                 return Ok(new { message = "Értesítési visszaigazolás elmentve" });
             }
             catch (ArgumentException ex)
             {
                 _logger.LogError(ex, "Hibás értesítési adat. A diák nem található vagy az értesítés nem létezik.");
                 return BadRequest(new { message = "Hibás paraméterek. Kérem, ellenőrizze a megadott adatokat." });
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Hiba történt az értesítés mentése során.");
                 return StatusCode(500, new { message = "Belső rendszerhiba történt. Kérem próbálja újra később." });
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