using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/customers";

    public CustomerService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<List<CustomerDto>>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<List<CustomerDto>>>(json)!;
    }

    public async Task<Response<CustomerDto>> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<CustomerDto>>(json)!;
    }

    public async Task<Response<CustomerDto>> CreateAsync(CustomerDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<CustomerDto>>(jsonResponse)!;
    }

    public async Task<Response<CustomerDto>> UpdateAsync(CustomerDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<CustomerDto>>(jsonResponse)!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<bool>>(jsonResponse)!;
    }
}