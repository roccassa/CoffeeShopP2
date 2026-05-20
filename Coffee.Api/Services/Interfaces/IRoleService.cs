using Coffee.Core.Dto;

namespace Coffee.Api.Services.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(int id);
    Task<RoleDto> SaveAsync(RoleDto roleDto);
    Task<RoleDto> UpdateAsync(RoleDto roleDto);
    Task<bool> DeleteAsync(int id);
}