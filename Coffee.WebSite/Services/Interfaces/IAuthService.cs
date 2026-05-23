using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IAuthService
{
    Task<Response<UserDto>> LoginAsync(string username, string password);
}
