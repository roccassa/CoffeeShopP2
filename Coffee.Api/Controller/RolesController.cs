using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _repository;

    public RolesController(IRoleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<RoleDto>>>> GetAll()
    {
        var roles = await _repository.GetAllAsync();
        var response = new Response<List<RoleDto>>
        {
            Data = roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<RoleDto>>> GetById(int id)
    {
        var response = new Response<RoleDto>();
        var role = await _repository.GetByIdAsync(id);
        if (role == null)
        {
            response.Errors.Add("Role not found");
            return NotFound(response);
        }
        response.Data = new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<RoleDto>>> Create([FromBody] RoleDto dto)
    {
        var response = new Response<RoleDto>();
        var role = new Coffee.Core.Entities.Role
        {
            Name = dto.Name
        };
        var result = await _repository.SaveAsync(role);
        if (!result)
        {
            response.Errors.Add("Error creating role");
            return BadRequest(response);
        }
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<RoleDto>>> Update(int id, [FromBody] RoleDto dto)
    {
        var response = new Response<RoleDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Role not found");
            return NotFound(response);
        }
        var role = new Coffee.Core.Entities.Role
        {
            Id = id,
            Name = dto.Name
        };
        var result = await _repository.UpdateAsync(role);
        if (!result)
        {
            response.Errors.Add("Error updating role");
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
            response.Errors.Add("Error deleting role");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}