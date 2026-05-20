using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IOrderDetailService
{
    Task<Response<List<OrderDetailDto>>> GetAllAsync();
    Task<Response<OrderDetailDto>> GetByIdAsync(int id);
    Task<Response<OrderDetailDto>> CreateAsync(OrderDetailDto dto);
    Task<Response<OrderDetailDto>> UpdateAsync(OrderDetailDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
