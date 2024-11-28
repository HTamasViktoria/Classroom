using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(new { message = ex.Message });
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
            return Ok(message);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = "Internal server error: " + e.Message });
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = "Internal server error: " + e.Message });
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = "Internal server error: " + e.Message });
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = "Internal server error: " + e.Message });
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
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return NotFound(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new { message = "Internal server error: " + e.Message }); // 500-as válasz
        }
    }


    [HttpPost("add")]
    public IActionResult Post([FromBody] MessageRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState });
        }
        try
        {
            _messagesRepository.AddMessage(request);
            return Ok(new { message = "Üzenet sikeresen elmentve az adatbázisba." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hibás üzenet-adat történt az 'Add' metódusban.");
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
                return BadRequest(new { message = "Hiba történt az üzenet törlésekor, vagy az üzenet nem található." });
            }
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Nem található üzenet ilyen id-val.");
            return BadRequest(new { message = ex.Message });
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
        try
        {
            var result = _messagesRepository.Restore(messageId, userId);

            return Ok(new { message = "Üzenet sikeresen visszaállítva." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Nem megfelelő userId vagy messageId");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet visszaállításakor.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }


    [HttpPost("setToUnread/{messageId}")]
    public IActionResult SetToUnread(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToUnread(messageId);

            return Ok(new { message = "Üzenet sikeresen olvasatlanná állítva." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasatlanná állításakor.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasatlanná állításakor.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }


    [HttpGet("setToRead/{messageId}")]
    public IActionResult SetToRead(int messageId)
    {
        try
        {
            var result = _messagesRepository.SetToRead(messageId);

            return Ok(new { message = "Üzenet sikeresen olvasottá állítva." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasottá állításakor.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hiba történt az üzenet olvasottá állításakor.");
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
}