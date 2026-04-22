using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController : ControllerBase
{
    private readonly IProductVariantRepository _repoVariants;
    public ProductVariantsController(IProductVariantRepository repo) => _repoVariants = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repoVariants.GetAllAsync());
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var variant = await _repoVariants.GetByIdAsync(id);
        if (variant == null) return NotFound();
        return Ok(variant);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductVariant variant)
    {
        if (variant.Price <= 0) return BadRequest("The price must be greater than 0.");
        if (string.IsNullOrWhiteSpace(variant.Size)) return BadRequest("Size is required.");
        if (variant.ProductId <= 0) return BadRequest("You must associate a valid Product.");

        try {
            return await _repoVariants.SaveAsync(variant) ? Ok("Presentation created") : BadRequest("Error creating");
        }
        catch (Exception ex) {
            if (ex.Message.Contains("foreign key constraint fails")) 
                return BadRequest("The product ID does not exist.");
            return StatusCode(500, "Server Error.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductVariant variant)
    {
        if (variant.Id <= 0) return BadRequest("ID invalid.");
        return await _repoVariants.UpdateAsync(variant) ? Ok("Updated") : BadRequest("Could not update");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _repoVariants.DeleteAsync(id) ? Ok("Deleted") : NotFound();
}