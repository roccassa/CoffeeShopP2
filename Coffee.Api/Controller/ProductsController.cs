using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _productRepository.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (string.IsNullOrEmpty(product.Name)) return BadRequest("El nombre es requerido");
        var result = await _productRepository.SaveAsync(product);
        return result ? Ok("Producto creado") : BadRequest("Error al crear");
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Product product)
    {
        var result = await _productRepository.UpdateAsync(product);
        if (result) return Ok("Product updated successfully");
        return BadRequest("Error updating product");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productRepository.DeleteAsync(id);
        if (result) return Ok("Product deleted");
        return BadRequest("Error deleting product");
    }
}