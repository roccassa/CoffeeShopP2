using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/products";

    public ProductService(HttpClient client) => _client = client;

    public async Task<Response<List<ProductDto>>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<List<ProductDto>>>(json)!;
    }

    public async Task<Response<ProductDto>> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<ProductDto>>(json)!;
    }

    public async Task<Response<ProductDto>> CreateAsync(ProductDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var body = await res.Content.ReadAsStringAsync();
        try
        {
            return JsonConvert.DeserializeObject<Response<ProductDto>>(body)
                   ?? new Response<ProductDto> { Success = false, Message = "Respuesta vacía de la API." };
        }
        catch
        {
            return new Response<ProductDto> { Success = false, Message = $"La API devolvió texto inválido: {body[..Math.Min(200, body.Length)]}" };
        }
    }

    public async Task<Response<ProductDto>> UpdateAsync(ProductDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<ProductDto>>(jsonResponse)!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<bool>>(jsonResponse)!;
    }
}
