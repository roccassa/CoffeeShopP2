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
        if (string.IsNullOrWhiteSpace(customer.Email)) return BadRequest("Email is mandatory.");
        
        try {
            var result = await _customerRepository.SaveAsync(customer);
            return result ? Ok("Registered customer\n") : BadRequest("Error registering");
        }
        catch (Exception ex) {
            if (ex.Message.Contains("Duplicate entry")) return BadRequest("This email is already registered.");
            return StatusCode(500, "Server error.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        if (customer.Id <= 0) return BadRequest("ID invalid");
        var result = await _customerRepository.UpdateAsync(customer);
        return result ? Ok("Updated customer") : BadRequest("Could not update");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _customerRepository.DeleteAsync(id) ? Ok("Customer deleted") : NotFound();
}