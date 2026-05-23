using Coffee.Core.Dto;
using Coffee.Core.Http;
using Coffee.WebSite.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Coffee.WebSite.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _client;

    public AuthService(HttpClient client) => _client = client;

    public async Task<Response<UserDto>> LoginAsync(string username, string password)
    {
        var payload = JsonConvert.SerializeObject(new { username, password });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        try
        {
            var res  = await _client.PostAsync("api/auth/login", content);
            var body = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<UserDto>>(body)
                   ?? new Response<UserDto> { Success = false, Message = "Respuesta vacía del servidor." };
        }
        catch (Exception ex)
        {
            return new Response<UserDto> { Success = false, Message = $"Error de conexión: {ex.Message}" };
        }
    }
}
