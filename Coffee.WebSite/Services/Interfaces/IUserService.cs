using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IUserService
{
    Task<Response<List<UserDto>>> GetAllAsync();
    Task<Response<UserDto>> GetByIdAsync(int id);
    Task<Response<UserDto>> CreateAsync(UserDto dto);
    Task<Response<UserDto>> UpdateAsync(UserDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
