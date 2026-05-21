using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repository;

    public OrdersController(IOrderRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<OrderDto>>>> GetAll()
    {
        var orders = await _repository.GetAllAsync();
        var response = new Response<List<OrderDto>>
        {
            Data = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CustomerId = o.CustomerId,
                PaymentMethodId = o.PaymentMethodId,
                Total = o.Total,
                Status = o.Status,
                UserName = o.UserName,
                CustomerName = o.CustomerName,
                PaymentMethodName = o.PaymentMethodName,
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<OrderDto>>> GetById(int id)
    {
        var response = new Response<OrderDto>();
        var order = await _repository.GetByIdAsync(id);
        if (order == null)
        {
            response.Errors.Add("Order not found");
            return NotFound(response);
        }
        response.Data = new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CustomerId = order.CustomerId,
            PaymentMethodId = order.PaymentMethodId,
            Total = order.Total,
            Status = order.Status
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<OrderDto>>> Create([FromBody] OrderDto dto)
    {
        var response = new Response<OrderDto>();

        var order = new Coffee.Core.Entities.Order
        {
            UserId = dto.UserId,
            CustomerId = dto.CustomerId,
            PaymentMethodId = dto.PaymentMethodId,
            Total = dto.Total,
            Status = dto.Status
        };

        var newId = await _repository.SaveAsync(order);

        if (newId == 0)
        {
            response.Success = false;
            response.Errors.Add("Error creating order");
            return BadRequest(response);
        }

        dto.Id = newId;

        response.Success = true;
        response.Message = "Orden creada correctamente";
        response.Data = dto;

        return Ok(response);
     
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<OrderDto>>> Update(int id, [FromBody] OrderDto dto)
    {
        var response = new Response<OrderDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Order not found");
            return NotFound(response);
        }
        var order = new Coffee.Core.Entities.Order
        {
            Id = id,
            UserId = dto.UserId,
            CustomerId = dto.CustomerId,
            PaymentMethodId = dto.PaymentMethodId,
            Total = dto.Total,
            Status = dto.Status
        };
        var result = await _repository.UpdateAsync(order);
        if (!result)
        {
            response.Errors.Add("Error updating order");
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
            response.Errors.Add("Error deleting order");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}