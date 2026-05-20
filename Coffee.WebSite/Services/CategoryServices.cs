using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class CategoryServices : ICategoryServices
{
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/categories";

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        var client = new HttpClient();
        var res = await client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<List<CategoryDto>>>(json);
        return response!;
    }

    public async Task<Response<CategoryDto>> GetByIdAsync(int id)
    {
        var client = new HttpClient();
        var res = await client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(json);
        return response!;
    }

    public async Task<Response<CategoryDto>> SaveAsync(CategoryDto dto)
    {
        var client = new HttpClient();
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(jsonResponse);
        return response!;
    }

    public async Task<Response<CategoryDto>> UpdateAsync(CategoryDto dto)
    {
        var client = new HttpClient();
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(jsonResponse);
        return response!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var client = new HttpClient();
        var res = await client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var jsonResponse = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<bool>>(jsonResponse);
        return response!;
    }
}