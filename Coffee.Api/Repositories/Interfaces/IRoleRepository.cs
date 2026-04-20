using Coffee.Core.Entities;
namespace Coffee.Api.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<bool> SaveAsync(Role role);
}