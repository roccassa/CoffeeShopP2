using Coffee.Core.Dto;

namespace Coffee.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerDto> SaveAsync(CustomerDto customerDto);
    Task<CustomerDto> UpdateAsync(CustomerDto customerDto);
    Task<bool> DeleteAsync(int id);
}