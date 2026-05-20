using System.Text;
using Newtonsoft.Json;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/Customers";

    public CustomerService(HttpClient client) => _client = client;

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<CustomerDto>>(json) ?? new List<CustomerDto>();
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CustomerDto>(json)!;
    }

    public async Task<CustomerDto> CreateAsync(CustomerDto customerDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(customerDto), Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        if (!res.IsSuccessStatusCode)
        {
            var error = await res.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        return customerDto;
    }

    public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(customerDto), Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}", content);
        if (!res.IsSuccessStatusCode)
            throw new Exception("Error al actualizar el cliente");
        return customerDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        return res.IsSuccessStatusCode;
    }
}