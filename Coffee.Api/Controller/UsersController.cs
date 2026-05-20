using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UsersController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<UserDto>>>> GetAll()
    {
        var users = await _repository.GetAllAsync();
        var response = new Response<List<UserDto>>
        {
            Data = users.Select(u => new UserDto
            {
                Id = u.Id,
                RoleId = u.RoleId,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<UserDto>>> GetById(int id)
    {
        var response = new Response<UserDto>();
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            response.Errors.Add("User not found");
            return NotFound(response);
        }
        response.Data = new UserDto
        {
            Id = user.Id,
            RoleId = user.RoleId,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            FullName = user.FullName
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<UserDto>>> Create([FromBody] UserDto dto)
    {
        var response = new Response<UserDto>();
        var user = new Coffee.Core.Entities.User
        {
            RoleId = dto.RoleId,
            Username = dto.Username,
            PasswordHash = dto.PasswordHash,
            FullName = dto.FullName
        };
        var result = await _repository.SaveAsync(user);
        if (!result)
        {
            response.Errors.Add("Error creating user");
            return BadRequest(response);
        }
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<UserDto>>> Update(int id, [FromBody] UserDto dto)
    {
        var response = new Response<UserDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("User not found");
            return NotFound(response);
        }
        var user = new Coffee.Core.Entities.User
        {
            Id = id,
            RoleId = dto.RoleId,
            Username = dto.Username,
            PasswordHash = dto.PasswordHash,
            FullName = dto.FullName
        };
        var result = await _repository.UpdateAsync(user);
        if (!result)
        {
            response.Errors.Add("Error updating user");
            return BadRequest(response);
        }
        dto.Id = id;
        response.Data = dto;
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var response = new Response<bool>();
        var result = await _repository.DeleteAsync(id);
        if (!result)
        {
            response.Errors.Add("Error deleting user");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}