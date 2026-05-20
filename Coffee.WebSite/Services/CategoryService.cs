using System.Text;
using Newtonsoft.Json;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

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
        return JsonConvert.DeserializeObject<List<CategoryDto>>(json) ?? new List<CategoryDto>();
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CategoryDto>(json);
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CategoryDto>(json);
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}", content);
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CategoryDto>(json);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        return res.IsSuccessStatusCode;
    }
}