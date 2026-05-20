using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<CustomerDto>>>> GetAll()
    {
        var customers = await _repository.GetAllAsync();
        var response = new Response<List<CustomerDto>>
        {
            Data = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                LoyaltyPoints = c.LoyaltyPoints
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<CustomerDto>>> GetById(int id)
    {
        var response = new Response<CustomerDto>();
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
        {
            response.Errors.Add("Customer not found");
            return NotFound(response);
        }
        response.Data = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            LoyaltyPoints = customer.LoyaltyPoints
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<CustomerDto>>> Create([FromBody] CustomerDto dto)
    {
        var response = new Response<CustomerDto>();
        var customer = new Coffee.Core.Entities.Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            LoyaltyPoints = dto.LoyaltyPoints
        };
        var result = await _repository.SaveAsync(customer);
        if (!result)
        {
            response.Errors.Add("Error creating customer");
            return BadRequest(response);
        }
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<CustomerDto>>> Update(int id, [FromBody] CustomerDto dto)
    {
        var response = new Response<CustomerDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Customer not found");
            return NotFound(response);
        }
        var customer = new Coffee.Core.Entities.Customer
        {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            LoyaltyPoints = dto.LoyaltyPoints
        };
        var result = await _repository.UpdateAsync(customer);
        if (!result)
        {
            response.Errors.Add("Error updating customer");
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
            response.Errors.Add("Error deleting customer");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}