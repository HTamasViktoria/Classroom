using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Classroom.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentController : ControllerBase
{
    private readonly ILogger<ParentController> _logger;
    private readonly IParentRepository _parentRepository;

    public ParentController(ILogger<ParentController> logger, IParentRepository parentRepository)
    {
        _logger = logger;
        _parentRepository = parentRepository;
    }
    
    
    [HttpGet("{id}", Name = "GetByParentId")]
    public ActionResult<Parent> GetByTeacherId(string id)
    {
        try
        {
            var parent = _parentRepository.GetParentById(id);
                
            if (parent == null)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }
            return Ok(parent);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
        
        
    [HttpGet(Name = "parents")]
    public ActionResult<IEnumerable<Parent>> GetAllParents()
    {
        try
        {
            return Ok(_parentRepository.GetAllParents());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}