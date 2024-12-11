using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Classroom.Service;


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
    public ActionResult<Parent> GetByParentId(string id)
    {
        
        try
        {
            var parent = _parentRepository.GetParentById(id);

            if (parent == null)
            {
                return StatusCode(400, $"Bad request: No parent found with the given id");
            }
        
            return Ok(parent);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
 
    [HttpGet("bystudent/{id}", Name = "GetByParentByStudentId")]
    public ActionResult<IEnumerable<Parent>> GetParentsByStudentId(string id)
    {
        try
        {
            var parents = _parentRepository.GetParentsByStudentId(id);

            if (!parents.Any())
            {
                return Ok(new List<Parent>());
            }

            return Ok(parents);
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, e.Message);
            return StatusCode(400, $"Bad request: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving parents.");
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }



        
    [HttpGet(Name = "parents")]
    public ActionResult<IEnumerable<Parent>> GetAllParents()
    {
        try
        {
            var parents = _parentRepository.GetAllParents();

            if (!parents.Any())
            {
                return Ok(new List<Parent>());
            }

            return Ok(parents);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}