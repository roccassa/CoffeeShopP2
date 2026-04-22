using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _userRepository.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        // Validación rigurosa de campos
        if (string.IsNullOrWhiteSpace(user.Username)) return BadRequest("A username is required.");
        if (string.IsNullOrWhiteSpace(user.PasswordHash)) return BadRequest("Password is required.");
        if (user.RoleId <= 0) return BadRequest("You must assign a valid role.");

        try 
        {
            var result = await _userRepository.SaveAsync(user);
            return result ? Ok("User created successfully.") : BadRequest("The user could not be created..");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Duplicate entry"))
                return BadRequest("The username already exists.");
            
            if (ex.Message.Contains("foreign key constraint fails"))
                return BadRequest("The provided Role ID does not exist.");

            return StatusCode(500, "Internal Server Error.");
        }
    }

    // Endpoint adicional para el login (Control y Validación)
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null || user.PasswordHash != password) // En un proyecto real usarías hashing
            return Unauthorized("Incorrect username or password.");

        return Ok(new { Message = $"Welcome {user.FullName}", Rol = user.RoleId });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user.Id <= 0) return BadRequest("Invalid user ID");
        var result = await _userRepository.UpdateAsync(user);
        return result ? Ok("User updated") : BadRequest("Could not update");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userRepository.DeleteAsync(id);
        return result ? Ok("User deleted") : NotFound("The user does not exist");
    }
}