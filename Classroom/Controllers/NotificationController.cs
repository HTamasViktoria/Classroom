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
    private readonly ILogger<NotificationController> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;

    public NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository, INotificationService notificationService)
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
            var notifications = _notificationRepository.GetAll();
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
    
    
    
    [HttpGet("bystudent/{studentId}/byparent/{parentId}", Name = "GetByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetByStudentId(string studentId, string parentId)
    {
        StringValidationHelper.IsValidId(studentId);
        StringValidationHelper.IsValidId(parentId);
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

    
    [HttpGet("teacher/{id}", Name = "GetTeachersNotifications")]
    public ActionResult<IEnumerable<NotificationBase>> GetByTeacherId(string id)
    {
        StringValidationHelper.IsValidId(id);
        try
        {
            var notifications = _notificationRepository.GetByTeacherId(id);
            if (!notifications.Any())
            {
                return Ok(new List<NotificationBase>());
            }
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Nincs ilyen azonosító:{id}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítések lekérdezése közben.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("studentslasts/{studentId}/{parentId}", Name = "GetLastsByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetLastsByStudentId(string studentId, string parentId)
    {
        StringValidationHelper.IsValidId(studentId);
        StringValidationHelper.IsValidId(parentId);
        try
        {
            var notifications = _notificationRepository.GetLastsByStudentId(studentId, parentId);
            
            if (!notifications.Any())
            {
                return Ok(new List<NotificationBase>());
            }
            
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Érvénytelen azonosító: {studentId} vagy {parentId}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítések lekérdezése közben.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    

    [HttpGet("newnotifsnum/{studentId}/{parentId}", Name = "GetNewNotifsNumber")]
    public ActionResult<int> GetNewNotifsNumber(string studentId, string parentId)
    {
        
        StringValidationHelper.IsValidId(studentId);
        StringValidationHelper.IsValidId(parentId);
        try
        {
            var newNotifsNumber = _notificationRepository.GetNewNotifsNumber(studentId, parentId);
            
            return Ok(newNotifsNumber);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Érvénytelen azonosító: {studentId} vagy {parentId}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítések számának lekérdezése közben.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    
    [HttpGet("newnotifs/{studentId}/{parentId}", Name = "GetNewNotifsByStudentId")]
    public ActionResult<IEnumerable<NotificationBase>> GetNewNotifsByStudentId(string studentId, string parentId)
    {
        
        StringValidationHelper.IsValidId(studentId);
        StringValidationHelper.IsValidId(parentId);
        try
        {
            var notifications = _notificationRepository.GetNewNotifsByStudentId(studentId, parentId);
            if (!notifications.Any())
            {
                return Ok(new List<NotificationBase>());
            }
            
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Érvénytelen azonosító: {studentId} vagy {parentId}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az új értesítések lekérdezése közben.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    
    [HttpGet("teacherslasts/{id}", Name = "GetLastsByTeacherId")]
    public ActionResult<IEnumerable<NotificationResponse>> GetLastsByTeacherId(string id)
    {
        
        StringValidationHelper.IsValidId(id);
        try
        {
            var notifications = _notificationRepository.GetLastsByTeacherId(id);
            
            if (!notifications.Any())
            {
                return Ok(new List<NotificationResponse>());
            }
            
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Érvénytelen azonosító:{id}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítések lekérdezése közben.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("teachersnewest/{id}", Name = "GetNewestByTeacherId")]
    public ActionResult<NotificationBase?> GetNewestByTeacherId(string id)
    {
        
        StringValidationHelper.IsValidId(id);
        try
        {
            var notification = _notificationRepository.GetNewestByTeacherId(id);
            if (notification == null)
            {
                return Ok(new NotificationBase());
            }

            return Ok(notification);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"Érvénytelen azonosító:{id}");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpPost]
    public IActionResult Post([FromBody] NotificationRequest request)
    {
      
        try
        {
            _notificationService.PostToDb(request);
            return CreatedAtAction(nameof(GetByTeacherId), new { id = request.TeacherId }, new { message = "Értesítés sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás értesítési adat történt a 'Post' metódusban.");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során a 'Post' metódusban.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    
    [HttpPost("read/{id}")]
    public IActionResult SetToRead(int id)
    {
        try
        {
            _notificationRepository.SetToRead(id);
            return StatusCode(200, $"Értesítés olvasottra állítva");
            
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás értesítési adat.");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
    [HttpPost("officiallyread/{id}")]
    public IActionResult SetToOfficiallyRead(int id)
    {
        try
        {
            _notificationRepository.SetToOfficiallyRead(id);
            return StatusCode(200, $"Értesítés olvasottra állítva");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás értesítési adat: Az értesítés nem található.");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az értesítés mentése során.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

         
   
    
    
    [HttpGet("homeworks", Name = "GetHomeworks")]
    public ActionResult<IEnumerable<NotificationBase>> GetHomeworks()
    {
        try
        {
            var homeworks = _notificationRepository.GetHomeworks();
            if (!homeworks.Any())
            {
                return Ok(new List<NotificationBase>());
            }
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
            if (!exams.Any())
            {
                return Ok(new List<NotificationBase>());
            }
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
            if (!others.Any())
            {
                return Ok(new List<NotificationBase>());
            }
            return Ok(others);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("missingequipments", Name = "GetMissingEquipments")]
    public ActionResult<IEnumerable<NotificationBase>> GetMissingEquipments()
    {
        try
        {
            var missingEquipments = _notificationRepository.GetMissingEquipments();
            if (!missingEquipments.Any())
            {
                return Ok(new List<NotificationBase>());
            }
            return Ok(missingEquipments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            _notificationRepository.Delete(id);
            return Ok(new { message = "Notification successfully deleted." });
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { error = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode(500, new { error = "An internal server error occurred. Please try again later." });
        }
    }

}