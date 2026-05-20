using Coffee.Core.Dto;

namespace Coffee.WebSite.Services.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(RoleDto roleDto);
    Task<RoleDto> UpdateAsync(RoleDto roleDto);
    Task<bool> DeleteAsync(int id);
}