using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

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
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        // Guardamos la entidad devuelta por el repositorio
        var createdCategory = await _categoryRepository.SaveAsync(category);
        
        // Evaluamos si el objeto no es nulo en lugar de buscar un bool
        if (createdCategory != null) 
        {
            // Siguiendo las buenas prácticas REST, devolvemos el objeto creado
            return Ok(createdCategory);
        }
        return BadRequest("Error creating category");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Category category)
    {
        // Guardamos la entidad devuelta por el repositorio
        var updatedCategory = await _categoryRepository.UpdateAsync(category);
        
        // Evaluamos si el objeto no es nulo
        if (updatedCategory != null) 
        {
            return Ok(updatedCategory);
        }
        return BadRequest("Error updating category");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryRepository.DeleteAsync(id);
        if (result) return Ok("Category deleted");
        return BadRequest("Error deleting category");
    }
}