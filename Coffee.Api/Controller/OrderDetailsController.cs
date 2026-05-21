using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly IOrderDetailRepository _repository;

    public OrderDetailsController(IOrderDetailRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<OrderDetailDto>>>> GetAll()
    {
        var details = await _repository.GetAllAsync();
        var response = new Response<List<OrderDetailDto>>
        {
            Data = details.Select(d => new OrderDetailDto
            {
                Id = d.Id,
                OrderId = d.OrderId,
                ProductVariantId = d.ProductVariantId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                PresentationName = d.PresentationName
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<OrderDetailDto>>> GetById(int id)
    {
        var response = new Response<OrderDetailDto>();
        var detail = await _repository.GetByIdAsync(id);
        if (detail == null)
        {
            response.Errors.Add("Order detail not found");
            return NotFound(response);
        }
        response.Data = new OrderDetailDto
        {
            Id = detail.Id,
            OrderId = detail.OrderId,
            ProductVariantId = detail.ProductVariantId,
            Quantity = detail.Quantity,
            UnitPrice = detail.UnitPrice
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<OrderDetailDto>>> Create([FromBody] OrderDetailDto dto)
    {
        var response = new Response<OrderDetailDto>();
        var detail = new Coffee.Core.Entities.OrderDetail
        {
            OrderId = dto.OrderId,
            ProductVariantId = dto.ProductVariantId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
        var result = await _repository.SaveAsync(detail);
        if (!result)
        {
            response.Success = false;

            response.Errors.Add("Error creating order detail");
            return BadRequest(response);
        }
        response.Success = true;
        response.Message = "Desglose creado correctamente";
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<OrderDetailDto>>> Update(int id, [FromBody] OrderDetailDto dto)
    {
        var response = new Response<OrderDetailDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Order detail not found");
            return NotFound(response);
        }
        var detail = new Coffee.Core.Entities.OrderDetail
        {
            Id = id,
            OrderId = dto.OrderId,
            ProductVariantId = dto.ProductVariantId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
        var result = await _repository.UpdateAsync(detail);
        if (!result)
        {
            response.Errors.Add("Error updating order detail");
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
            response.Errors.Add("Error deleting order detail");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}