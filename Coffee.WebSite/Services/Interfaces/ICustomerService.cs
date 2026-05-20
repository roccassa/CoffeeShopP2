using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface ICustomerService
{
    Task<Response<List<CustomerDto>>> GetAllAsync();
    Task<Response<CustomerDto>> GetByIdAsync(int id);
    Task<Response<CustomerDto>> CreateAsync(CustomerDto dto);
    Task<Response<CustomerDto>> UpdateAsync(CustomerDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}