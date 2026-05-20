using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IRoleService
{
    Task<Response<List<RoleDto>>> GetAllAsync();
    Task<Response<RoleDto>> GetByIdAsync(int id);
    Task<Response<RoleDto>> CreateAsync(RoleDto dto);
    Task<Response<RoleDto>> UpdateAsync(RoleDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}