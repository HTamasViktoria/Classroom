using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;

namespace Classroom.Controllers;

[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMessagesRepository _messagesRepository;


    public MessagesController(ILogger<MessagesController> logger, IMessagesRepository messagesRepository)
    {
        _logger = logger;
        _messagesRepository = messagesRepository;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllMessagesAsync()
    {
        try
        {
            var messages = await _messagesRepository.GetAllMessagesAsync();
            if (!messages.Any())
            {
                return Ok(new List<Message>());
            }

            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenetek lekérésekor.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("newmessagesnum/{id}", Name = "GetNewMessagesNum")]
    public ActionResult<int> GetNewMessagesNum(string id)
    {
        try
        {
            var newMessagesNum = _messagesRepository.GetNewMessagesNum(id);
            return Ok(newMessagesNum);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(404, $"ˇNot found: {ex.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("{id}", Name = "GetById")]
    public ActionResult<Message> GetById(int id)
    {
        try
        {
            var message = _messagesRepository.GetById(id);
            return Ok(message);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(404, $"ˇNot found: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("incomings/{id}", Name = "GetIncomings")]
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(404, $"ˇNot found: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("deleteds/{id}", Name = "GetDeleteds")]
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(404, $"ˇNot found: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("sents/{id}", Name = "GetSents")]
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(404, $"ˇNot found: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpGet("outgoings/{id}", Name = "GetOutgoings")]
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(404, $"ˇNot found: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }


    [HttpPost]
    public IActionResult Post([FromBody] MessageRequest request)
    {
       
        try
        {
            _messagesRepository.AddMessage(request);
            return CreatedAtAction(nameof(Post), new { id = request.Id }, new { message = "Üzenet sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás üzenet-adat történt az 'Add' metódusban.");
            return StatusCode(400, $"Bad request:{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet mentése során a 'Add' metódusban.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("receiverdelete/{messageId}")]
    public IActionResult DeleteMessageOnReceiverSide(int messageId)
    {
        try
        {
            var result = _messagesRepository.DeleteOnReceiverSide(messageId);

                return StatusCode(200,"Üzenet sikeresen törölve a fogadó fél oldalán.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Nem található üzenet ilyen ID-val.");
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet törlésénél.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
    
    

    [HttpGet("restore/{messageId}/{userId}")]
    public IActionResult Restore(int messageId, string userId)
    {
        
        try
        {
            var result = _messagesRepository.Restore(messageId, userId);

            return StatusCode(200,"Üzenet sikeresen visszaállítva.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Nem megfelelő userId vagy messageId");
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet visszaállításakor.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpPost("unread/{messageId}")]
    public IActionResult SetToUnread(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToUnread(messageId);

            return StatusCode(200,"Üzenet sikeresen olvasatlanra állítva.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasatlanná állításakor.");
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasatlanná állításakor.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("read/{messageId}")]
    public IActionResult SetToRead(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToRead(messageId);

            return StatusCode(200,"Üzenet sikeresen olvasottra állítva.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasottá állításakor.");
            return StatusCode(400, $"Bad request: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasottá állításakor.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}