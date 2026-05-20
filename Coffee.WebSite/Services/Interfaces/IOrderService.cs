using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IOrderService
{
    Task<Response<List<OrderDto>>> GetAllAsync();
    Task<Response<OrderDto>> GetByIdAsync(int id);
    Task<Response<OrderDto>> CreateAsync(OrderDto dto);
    Task<Response<OrderDto>> UpdateAsync(OrderDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
