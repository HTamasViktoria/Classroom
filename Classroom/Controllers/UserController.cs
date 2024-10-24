using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Classroom.Service.Repositories;
using Classroom.Model.DataModels;
using System.Collections.Generic;

namespace Classroom.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        
        [HttpGet("teachers")]
        public ActionResult<IEnumerable<Teacher>> GetAllTeachers()
        {
            try
            {
                var teachers = _userRepository.GetAllTeachers();
                return Ok(teachers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("parents")]
        public ActionResult<IEnumerable<Parent>> GetAllParents()
        {
            try
            {
                var parents = _userRepository.GetAllParents();
                return Ok(parents);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("teachers/{teacherId}")]
        public ActionResult<Teacher> GetTeacherById(string teacherId)
        {
            try
            {
                var teacher = _userRepository.GetTeacherById(teacherId);
                if (teacher == null) return NotFound($"Teacher with ID {teacherId} not found.");
                return Ok(teacher);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

     
        [HttpGet("parents/{parentId}")]
        public ActionResult<Parent> GetParentById(string parentId)
        {
            try
            {
                var parent = _userRepository.GetParentById(parentId);
                if (parent == null) return NotFound($"Parent with ID {parentId} not found.");
                return Ok(parent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpPost("teachers")]
        public ActionResult<string> AddTeacher([FromBody] Teacher teacher)
        {
            try
            {
                _userRepository.AddTeacher(teacher);
                return CreatedAtAction(nameof(GetTeacherById), new { teacherId = teacher.Id }, teacher);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpPost("parents")]
        public ActionResult<string> AddParent([FromBody] Parent parent)
        {
            try
            {
                _userRepository.AddParent(parent);
                return CreatedAtAction(nameof(GetParentById), new { parentId = parent.Id }, parent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Internal server error");
            }
        }

       
    }
}
