using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/Categories";

    public CategoryService(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> CategoryExistsAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        return res.IsSuccessStatusCode;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<List<CategoryDto>>>(json);
        return response?.Data ?? new List<CategoryDto>();
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(json);
        return response?.Data!;
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(json);
        return response?.Data!;
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{categoryDto.Id}", content);
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<CategoryDto>>(json);
        return response?.Data!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<bool>>(json);
        return response?.Data ?? false;
    }
}