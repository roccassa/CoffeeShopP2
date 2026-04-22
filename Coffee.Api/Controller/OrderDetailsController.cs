using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase {
    private readonly IOrderDetailRepository _detailRepo;
    public OrderDetailsController(IOrderDetailRepository detailRepo) => _detailRepo = detailRepo;

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] OrderDetail detail) {
        if (detail.Quantity <= 0 || detail.UnitPrice <= 0) return BadRequest("Quantity and price must be greater than 0.");
        return await _detailRepo.SaveAsync(detail) ? Ok("Product added to order") : BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _detailRepo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var detail = await _detailRepo.GetByIdAsync(id);
        return detail == null ? NotFound() : Ok(detail);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderDetail detail) {
        if (detail.Id <= 0) return BadRequest("ID invalid");
        return await _detailRepo.UpdateAsync(detail) ? Ok("Updated detail") : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        return await _detailRepo.DeleteAsync(id) ? Ok("Detail removed") : NotFound();
    }
}