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
        if (string.IsNullOrWhiteSpace(user.Username)) return BadRequest("El nombre de usuario es requerido.");
        if (string.IsNullOrWhiteSpace(user.PasswordHash)) return BadRequest("La contraseña es requerida.");
        if (user.RoleId <= 0) return BadRequest("Debe asignar un rol válido.");

        try 
        {
            var result = await _userRepository.SaveAsync(user);
            return result ? Ok("Usuario creado exitosamente.") : BadRequest("No se pudo crear el usuario.");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Duplicate entry"))
                return BadRequest("El nombre de usuario ya existe.");
            
            if (ex.Message.Contains("foreign key constraint fails"))
                return BadRequest("El ID de Rol proporcionado no existe.");

            return StatusCode(500, "Error interno del servidor.");
        }
    }

    // Endpoint adicional para el login (Control y Validación)
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null || user.PasswordHash != password) // En un proyecto real usarías hashing
            return Unauthorized("Usuario o contraseña incorrectos.");

        return Ok(new { Message = $"Bienvenido {user.FullName}", Rol = user.RoleId });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound("Usuario no encontrado");
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user.Id <= 0) return BadRequest("ID de usuario no válido");
        var result = await _userRepository.UpdateAsync(user);
        return result ? Ok("Usuario actualizado") : BadRequest("No se pudo actualizar");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userRepository.DeleteAsync(id);
        return result ? Ok("Usuario eliminado") : NotFound("El usuario no existe");
    }
}