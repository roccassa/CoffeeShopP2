using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;
namespace Coffee.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase {
    private readonly IOrderRepository _orderRepo;
    public OrdersController(IOrderRepository orderRepo) => _orderRepo = orderRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _orderRepo.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order) {
        if (order.UserId <= 0 || order.PaymentMethodId <= 0) return BadRequest("Incomplete order data.");
        var id = await _orderRepo.SaveAsync(order);
        return Ok(new { Message = "Order started", OrderId = id });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var order = await _orderRepo.GetByIdAsync(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Order order) {
        if (order.Id <= 0) return BadRequest("ID invalid");
        return await _orderRepo.UpdateAsync(order) ? Ok("Order updated") : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        return await _orderRepo.DeleteAsync(id) ? Ok("Order deleted") : NotFound();
    }
}