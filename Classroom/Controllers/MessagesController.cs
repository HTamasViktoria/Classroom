using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Classroom.Controllers;



[ApiController]
[Route("api/messages")]
public class MessagesController: ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMessagesRepository _messagesRepository;
    
    
    
    public MessagesController(ILogger<MessagesController> logger, IMessagesRepository messagesRepository)
    {
        _logger = logger;
        _messagesRepository = messagesRepository;
       
    }
    
    [HttpGet("getall")]
    public IActionResult GetAllMessages()
    {
        try
        {
            var messages = _messagesRepository.GetAllMessages();
            
            if (!messages.Any())
            {
                return NotFound(new { message = "Nincsenek üzenetek." });
            }
            
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenetek lekérésekor.");
            return StatusCode(500, new { message = "Internal server error: " + ex.Message });
        }
    }
    
    
    
    [HttpGet("getNewMessagesNum/{id}", Name = "GetNewMessagesNum")]
    public ActionResult<int> GetNewMessagesNum(string id)
    {
        try
        {
            var newMessagesNum = _messagesRepository.GetNewMessagesNum(id);
            return Ok(newMessagesNum);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    
    [HttpGet("getById/{id}", Name = "GetById")]
    public ActionResult<Message> GetById(int id)
    {
        try
        {
            var message = _messagesRepository.GetById(id);
        
            if (message == null)
            {
                return NotFound("Message not found");
            }
        
            return Ok(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }



    
    
    [HttpGet("getIncomings/{id}", Name = "GetIncomings")]
    public ActionResult<IEnumerable<Message>> GetIncomings(string id)
    {
        try
        {
            var messages = _messagesRepository.GetIncomings(id);
        
            if (!messages.Any())
            {
                return Ok(new List<Message>());
            }
        
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("getDeleteds/{id}", Name = "GetDeleteds")]
    public ActionResult<IEnumerable<Message>> GetDeleteds(string id)
    {
        try
        {
            var messages = _messagesRepository.GetDeleteds(id);
        
            if (!messages.Any())
            {
                return Ok(new List<Message>());
            }
        
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    
    [HttpGet("getSents/{id}", Name = "GetSents")]
    public ActionResult<IEnumerable<Message>> GetSents(string id)
    {
        try
        {
            var messages = _messagesRepository.GetSents(id);
        
            if (!messages.Any())
            {
                return Ok(new List<Message>());
            }
        
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpGet("getOutgoings/{id}", Name = "GetOutgoings")]
    public ActionResult<IEnumerable<Message>> GetOutgoings(string id)
    {
        try
        {
            var messages = _messagesRepository.GetOutgoings(id);
        
            if (!messages.Any())
            {
                return Ok(new List<Message>());
            }
        
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    
    [HttpPost("add")]
    public IActionResult Post([FromBody] MessageRequest request)
    {
        try
        {
            _messagesRepository.AddMessage(request);


            return Ok(new { message = "Üzenet sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás üzenet- adat történt az 'Add' metódusban.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet mentése során a 'Add' metódusban.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
    
    
    
    
    [HttpDelete("receiverDelete/{messageId}")]
    public IActionResult DeleteMessageOnReceiverSide(int messageId)
    {
        try
        {
            var result = _messagesRepository.DeleteOnReceiverSide(messageId);

            if (result)
            {
                return Ok(new { message = "Üzenet sikeresen törölve a fogadó fél oldalán." });
            }
            else
            {
                return NotFound(new { message = "Az üzenet nem található." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet törlésénél.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
    
    
    [HttpGet("restore/{messageId}/{userId}")]
    public IActionResult Restore(int messageId, string userId)
    {
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine($"messageId: {messageId}");

        try
        {
            var result = _messagesRepository.Restore(messageId, userId);

            if (result)
            {
                return Ok(new { message = "Üzenet sikeresen visszaállítva." });
            }
            else
            {
                return NotFound(new { message = "Az üzenet nem található vagy nem állítható vissza." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet visszaállításánál.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }

    
    
    [HttpGet("setToUnread/{messageId}/")]
    public IActionResult SetToUnread(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToUnread(messageId);

            if (result)
            {
                return Ok(new { message = "Üzenet sikeresen visszaállítva." });
            }
            else
            {
                return NotFound(new { message = "Az üzenet nem található." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet visszaállításánál.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
    
    
    
    [HttpGet("setToRead/{messageId}/")]
    public IActionResult SetToRead(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToRead(messageId);

            if (result)
            {
                return Ok(new { message = "Üzenet sikeresen visszaállítva." });
            }
            else
            {
                return NotFound(new { message = "Az üzenet nem található." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet visszaállításánál.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }


}


