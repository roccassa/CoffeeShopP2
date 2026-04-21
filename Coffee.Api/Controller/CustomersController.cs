using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository) => _customerRepository = customerRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _customerRepository.GetAllAsync());
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Email)) return BadRequest("El correo es obligatorio.");
        
        try {
            var result = await _customerRepository.SaveAsync(customer);
            return result ? Ok("Cliente registrado") : BadRequest("Error al registrar");
        }
        catch (Exception ex) {
            if (ex.Message.Contains("Duplicate entry")) return BadRequest("Ese correo ya está registrado.");
            return StatusCode(500, "Error de servidor.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        if (customer.Id <= 0) return BadRequest("ID inválido");
        var result = await _customerRepository.UpdateAsync(customer);
        return result ? Ok("Cliente actualizado") : BadRequest("No se pudo actualizar");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _customerRepository.DeleteAsync(id) ? Ok("Cliente eliminado") : NotFound();
}