using Coffee.Core.Entities;
namespace Coffee.Api.Repositories.Interfaces;

public interface IRoleRepository
{
    // Obtiene el catálogo completo de roles definidos en el sistema
    Task<IEnumerable<Role>> GetAllAsync();

    // Consulta un rol específico mediante su identificador único
    Task<Role?> GetByIdAsync(int id);

    // Registra un nuevo rol de usuario en la base de datos
    Task<bool> SaveAsync(Role role);

    // Actualiza el nombre o descripción de un rol existente
    Task<bool> UpdateAsync(Role role);

    // Elimina un rol del sistema de permisos
    Task<bool> DeleteAsync(int id);
}