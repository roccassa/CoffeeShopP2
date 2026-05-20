using System.Text;
using Newtonsoft.Json;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Services;

public class RoleService : IRoleService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://localhost:5140/";
    private readonly string _endpoint = "api/Roles";

    public RoleService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<RoleDto>>(json) ?? new List<RoleDto>();
    }

    public async Task<RoleDto> GetByIdAsync(int id)
    {
        var res = await _client.GetAsync($"{_baseUrl}{_endpoint}/{id}");
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RoleDto>(json)!;
    }

    public async Task<RoleDto> CreateAsync(RoleDto roleDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(roleDto), Encoding.UTF8, "application/json");
        var res = await _client.PostAsync($"{_baseUrl}{_endpoint}", content);
        // La API devuelve un string, no un objeto, así que solo verificamos éxito
        if (!res.IsSuccessStatusCode)
            throw new Exception("Error al crear el rol");
        return roleDto;
    }

    public async Task<RoleDto> UpdateAsync(RoleDto roleDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(roleDto), Encoding.UTF8, "application/json");
        var res = await _client.PutAsync($"{_baseUrl}{_endpoint}", content);
        if (!res.IsSuccessStatusCode)
            throw new Exception("Error al actualizar el rol");
        return roleDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _client.DeleteAsync($"{_baseUrl}{_endpoint}/{id}");
        return res.IsSuccessStatusCode;
    }
}