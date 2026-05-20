using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<CategoryDto>>>> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var response = new Response<List<CategoryDto>>
        {
            Data = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<CategoryDto>>> GetById(int id)
    {
        var response = new Response<CategoryDto>();
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            response.Errors.Add("Category not found");
            return NotFound(response);
        }
        response.Data = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<CategoryDto>>> Create([FromBody] CategoryDto dto)
    {
        var response = new Response<CategoryDto>();
        var category = new Coffee.Core.Entities.Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        var result = await _categoryRepository.SaveAsync(category);
        if (!result)
        {
            response.Errors.Add("Error creating category");
            return BadRequest(response);
        }
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<CategoryDto>>> Update(int id, [FromBody] CategoryDto dto)
    {
        var response = new Response<CategoryDto>();
        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Category not found");
            return NotFound(response);
        }
        var category = new Coffee.Core.Entities.Category
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description
        };
        var result = await _categoryRepository.UpdateAsync(category);
        if (!result)
        {
            response.Errors.Add("Error updating category");
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
        var result = await _categoryRepository.DeleteAsync(id);
        if (!result)
        {
            response.Errors.Add("Error deleting category");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}