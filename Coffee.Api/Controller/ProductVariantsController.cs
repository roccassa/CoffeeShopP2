using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController : ControllerBase
{
    private readonly IProductVariantRepository _repository;

    public ProductVariantsController(IProductVariantRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<ProductVariantDto>>>> GetAll()
    {
        var variants = await _repository.GetAllAsync();
        var response = new Response<List<ProductVariantDto>>
        {
            Data = variants.Select(v => new ProductVariantDto
            {
                Id          = v.Id,
                ProductId   = v.ProductId,
                ProductName = v.ProductName,
                Size        = v.Size,
                Price       = v.Price
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<ProductVariantDto>>> GetById(int id)
    {
        var response = new Response<ProductVariantDto>();
        var variant = await _repository.GetByIdAsync(id);
        if (variant == null)
        {
            response.Errors.Add("Product variant not found");
            return NotFound(response);
        }
        response.Data = new ProductVariantDto
        {
            Id = variant.Id,
            ProductId = variant.ProductId,
            Size = variant.Size,
            Price = variant.Price
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<ProductVariantDto>>> Create([FromBody] ProductVariantDto dto)
    {
        var response = new Response<ProductVariantDto>();
        var variant = new Coffee.Core.Entities.ProductVariant
        {
            ProductId = dto.ProductId,
            Size = dto.Size,
            Price = dto.Price
        };
        var result = await _repository.SaveAsync(variant);
        if (!result)
        {
            response.Success = false;
            response.Errors.Add("Error creating product variant");
            return BadRequest(response);
        }
        
        response.Success = true;
        response.Message = "Presentacion creada correctamente";
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<ProductVariantDto>>> Update(int id, [FromBody] ProductVariantDto dto)
    {
        var response = new Response<ProductVariantDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Product variant not found");
            return NotFound(response);
        }
        var variant = new Coffee.Core.Entities.ProductVariant
        {
            Id = id,
            ProductId = dto.ProductId,
            Size = dto.Size,
            Price = dto.Price
        };
        var result = await _repository.UpdateAsync(variant);
        if (!result)
        {
            response.Errors.Add("Error updating product variant");
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
            response.Errors.Add("Error deleting product variant");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}