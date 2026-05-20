using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.Services.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Entities;

namespace Coffee.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => new CustomerDto(c)).ToList();
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new Exception($"Customer with ID {id} not found");
        return new CustomerDto(customer);
    }

    public async Task<CustomerDto> SaveAsync(CustomerDto customerDto)
    {
        var customer = new Customer
        {
            Name = customerDto.Name,
            Email = customerDto.Email,
            LoyaltyPoints = customerDto.LoyaltyPoints
        };
        await _customerRepository.SaveAsync(customer);
        return customerDto;
    }

    public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerDto.Id);
        if (customer == null)
            throw new Exception($"Customer with ID {customerDto.Id} not found");

        customer.Name = customerDto.Name;
        customer.Email = customerDto.Email;
        customer.LoyaltyPoints = customerDto.LoyaltyPoints;

        await _customerRepository.UpdateAsync(customer);
        return new CustomerDto(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _customerRepository.DeleteAsync(id);
    }
}