using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodRepository _repo;
    public PaymentMethodsController(IPaymentMethodRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var method = await _repo.GetByIdAsync(id);
        if (method == null) return NotFound();
        return Ok(method);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentMethod method)
    {
        if (string.IsNullOrWhiteSpace(method.Name)) return BadRequest("El nombre del método de pago es obligatorio.");
        return await _repo.SaveAsync(method) ? Ok("Método de pago creado") : BadRequest("Error al crear");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PaymentMethod method)
    {
        if (method.Id <= 0) return BadRequest("ID inválido");
        return await _repo.UpdateAsync(method) ? Ok("Actualizado correctamente") : BadRequest("No se pudo actualizar");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _repo.DeleteAsync(id) ? Ok("Eliminado") : NotFound("No existe el ID");
}