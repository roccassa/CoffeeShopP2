using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class RoleService : IRoleService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/roles";

    public RoleService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Response<List<RoleDto>>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<List<RoleDto>>>(json)!;
    }

    public async Task<Response<RoleDto>> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<RoleDto>>(json)!;
    }

    public async Task<Response<RoleDto>> CreateAsync(RoleDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<RoleDto>>(jsonResponse)!;
    }

    public async Task<Response<RoleDto>> UpdateAsync(RoleDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}/{dto.Id}", content);
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<RoleDto>>(jsonResponse)!;
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        var jsonResponse = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<bool>>(jsonResponse)!;
    }
}