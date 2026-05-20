using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.Services.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Entities;

namespace Coffee.Api.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r => new RoleDto(r)).ToList();
    }

    public async Task<RoleDto> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new Exception($"Role with ID {id} not found");
        return new RoleDto(role);
    }

    public async Task<RoleDto> SaveAsync(RoleDto roleDto)
    {
        var role = new Role { Name = roleDto.Name };
        await _roleRepository.SaveAsync(role);
        return roleDto;
    }

    public async Task<RoleDto> UpdateAsync(RoleDto roleDto)
    {
        var role = await _roleRepository.GetByIdAsync(roleDto.Id);
        if (role == null)
            throw new Exception($"Role with ID {roleDto.Id} not found");

        role.Name = roleDto.Name;
        await _roleRepository.UpdateAsync(role);
        return new RoleDto(role);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _roleRepository.DeleteAsync(id);
    }
}