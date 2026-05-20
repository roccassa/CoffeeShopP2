using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodsController(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<PaymentMethodDto>>>> GetAll()
    {
        var methods = await _repository.GetAllAsync();
        var response = new Response<List<PaymentMethodDto>>
        {
            Data = methods.Select(m => new PaymentMethodDto
            {
                Id = m.Id,
                Name = m.Name
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<PaymentMethodDto>>> GetById(int id)
    {
        var response = new Response<PaymentMethodDto>();
        var method = await _repository.GetByIdAsync(id);
        if (method == null)
        {
            response.Errors.Add("Payment method not found");
            return NotFound(response);
        }
        response.Data = new PaymentMethodDto
        {
            Id = method.Id,
            Name = method.Name
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<PaymentMethodDto>>> Create([FromBody] PaymentMethodDto dto)
    {
        var response = new Response<PaymentMethodDto>();
        var method = new Coffee.Core.Entities.PaymentMethod
        {
            Name = dto.Name
        };
        var result = await _repository.SaveAsync(method);
        if (!result)
        {
            response.Errors.Add("Error creating payment method");
            return BadRequest(response);
        }
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<PaymentMethodDto>>> Update(int id, [FromBody] PaymentMethodDto dto)
    {
        var response = new Response<PaymentMethodDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Payment method not found");
            return NotFound(response);
        }
        var method = new Coffee.Core.Entities.PaymentMethod
        {
            Id = id,
            Name = dto.Name
        };
        var result = await _repository.UpdateAsync(method);
        if (!result)
        {
            response.Errors.Add("Error updating payment method");
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
            response.Errors.Add("Error deleting payment method");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}