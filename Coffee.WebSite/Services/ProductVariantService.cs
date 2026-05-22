using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class ProductVariantService : IProductVariantService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/productvariants";

    public ProductVariantService(HttpClient client) => _client = client;

    public async Task<Response<List<ProductVariantDto>>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<List<ProductVariantDto>>>(json)!;
    }

    public async Task<Response<ProductVariantDto>> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<ProductVariantDto>>(json)!;
    }

    public async Task<Response<ProductVariantDto>> CreateAsync(ProductVariantDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var body = await res.Content.ReadAsStringAsync();
        try
        {
            return JsonConvert.DeserializeObject<Response<ProductVariantDto>>(body)
                   ?? new Response<ProductVariantDto> { Success = false, Message = "Respuesta vacía de la API." };
        }
        catch
        {
            return new Response<ProductVariantDto> { Success = false, Message = $"La API devolvió texto inválido: {body[..Math.Min(200, body.Length)]}" };
        }
    }

    public async Task<Response<ProductVariantDto>> UpdateAsync(ProductVariantDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<ProductVariantDto>>(jsonResponse)!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var res  = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var body = await res.Content.ReadAsStringAsync();
        try
        {
            return JsonConvert.DeserializeObject<Response<bool>>(body)
                   ?? new Response<bool> { Success = false };
        }
        catch
        {
            return new Response<bool> { Success = false, Message = body[..Math.Min(200, body.Length)] };
        }
    }
}
