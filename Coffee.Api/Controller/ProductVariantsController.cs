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
        if (variant.Price <= 0) return BadRequest("El precio debe ser mayor a 0.");
        if (string.IsNullOrWhiteSpace(variant.Size)) return BadRequest("El tamaño (Size) es obligatorio.");
        if (variant.ProductId <= 0) return BadRequest("Debe asociar un Producto válido.");

        try {
            return await _repoVariants.SaveAsync(variant) ? Ok("Presentación creada") : BadRequest("Error al crear");
        }
        catch (Exception ex) {
            if (ex.Message.Contains("foreign key constraint fails")) 
                return BadRequest("El ID de producto no existe.");
            return StatusCode(500, "Error de servidor.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductVariant variant)
    {
        if (variant.Id <= 0) return BadRequest("ID inválido.");
        return await _repoVariants.UpdateAsync(variant) ? Ok("Actualizado") : BadRequest("No se pudo actualizar");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _repoVariants.DeleteAsync(id) ? Ok("Eliminado") : NotFound();
}