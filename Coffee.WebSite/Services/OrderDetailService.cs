using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/orderdetails";

    public OrderDetailService(HttpClient client) => _client = client;

    public async Task<Response<List<OrderDetailDto>>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        
        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error en OrderDetail.GetAllAsync ({res.StatusCode}): {json}");

        return JsonConvert.DeserializeObject<Response<List<OrderDetailDto>>>(json)!;
    }

    public async Task<Response<OrderDetailDto>> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        
        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error en OrderDetail.GetByIdAsync ({res.StatusCode}): {json}");

        return JsonConvert.DeserializeObject<Response<OrderDetailDto>>(json)!;
    }

    public async Task<Response<OrderDetailDto>> CreateAsync(OrderDetailDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        
        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error en OrderDetail.CreateAsync ({res.StatusCode}): {jsonResponse}");

        return JsonConvert.DeserializeObject<Response<OrderDetailDto>>(jsonResponse)!;
    }

    public async Task<Response<OrderDetailDto>> UpdateAsync(OrderDetailDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        
        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error en OrderDetail.UpdateAsync ({res.StatusCode}): {jsonResponse}");

        return JsonConvert.DeserializeObject<Response<OrderDetailDto>>(jsonResponse)!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var jsonResponse = await res.Content.ReadAsStringAsync();
        
        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error en OrderDetail.DeleteAsync ({res.StatusCode}): {jsonResponse}");

        return JsonConvert.DeserializeObject<Response<bool>>(jsonResponse)!;
    }
}