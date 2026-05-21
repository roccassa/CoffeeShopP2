using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<ProductDto>>>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        var response = new Response<List<ProductDto>>
        {
            Data = products.Select(p => new ProductDto
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                CategoryName = p.CategoryName,
                Description = p.Description,
                IsActive = p.IsActive
            }).ToList()
        };
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<ProductDto>>> GetById(int id)
    {
        var response = new Response<ProductDto>();
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            response.Errors.Add("Product not found");
            return NotFound(response);
        }
        response.Data = new ProductDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Description = product.Description,
            IsActive = product.IsActive
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<ProductDto>>> Create([FromBody] ProductDto dto)
    {
        var response = new Response<ProductDto>();
        var product = new Coffee.Core.Entities.Product
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
        var result = await _repository.SaveAsync(product);
        if (!result)
        {
            response.Success = false;
            response.Errors.Add("Error creating product");
            return BadRequest(response);
        }
        response.Success = true;
        response.Message = "Orden creada correctamente";
        response.Data = dto;
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<ProductDto>>> Update(int id, [FromBody] ProductDto dto)
    {
        var response = new Response<ProductDto>();
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            response.Errors.Add("Product not found");
            return NotFound(response);
        }
        var product = new Coffee.Core.Entities.Product
        {
            Id = id,
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
        var result = await _repository.UpdateAsync(product);
        if (!result)
        {
            response.Errors.Add("Error updating product");
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
            response.Errors.Add("Error deleting product");
            return BadRequest(response);
        }
        response.Data = true;
        return Ok(response);
    }
}