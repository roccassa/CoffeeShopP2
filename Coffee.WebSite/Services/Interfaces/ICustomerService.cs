using Coffee.Core.Dto;

namespace Coffee.WebSite.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CustomerDto customerDto);
    Task<CustomerDto> UpdateAsync(CustomerDto customerDto);
    Task<bool> DeleteAsync(int id);
}