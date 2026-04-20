using Coffee.Core.Entities;
namespace Coffee.Api.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task<bool> SaveAsync(Role role);
    Task<bool> UpdateAsync(Role role);
    Task<bool> DeleteAsync(int id);
}