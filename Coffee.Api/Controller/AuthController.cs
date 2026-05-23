using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;

    public AuthController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Response<UserDto>>> Login([FromBody] LoginRequest request)
    {
        var response = new Response<UserDto>();

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            response.Success = false;
            response.Message = "Usuario y contraseña son requeridos.";
            return Ok(response);
        }

        var user = await _repository.GetByUsernameAsync(request.Username.Trim());
        if (user == null)
        {
            response.Success = false;
            response.Message = "Usuario o contraseña incorrectos.";
            return Ok(response);
        }

        // Verificar contraseña: BCrypt si el hash empieza con "$2", de lo contrario texto plano
        bool valid = false;
        if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.StartsWith("$2"))
        {
            try { valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash); }
            catch { valid = false; }
        }
        else
        {
            // Contraseña guardada en texto plano (usuarios creados sin hash)
            valid = user.PasswordHash == request.Password;
        }

        if (!valid)
        {
            response.Success = false;
            response.Message = "Usuario o contraseña incorrectos.";
            return Ok(response);
        }

        response.Success = true;
        response.Data = new UserDto
        {
            Id       = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            RoleId   = user.RoleId,
            RoleName = user.RoleName
        };
        return Ok(response);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
