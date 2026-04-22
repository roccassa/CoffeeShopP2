using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IUserRepository
{
    // Obtiene la lista de todos los usuarios (empleados) registrados
    Task<IEnumerable<User>> GetAllAsync();

    // Busca un usuario específico por su identificador único
    Task<User?> GetByIdAsync(int id);

    // Busca un usuario por su nombre de acceso, esencial para el proceso de login
    Task<User?> GetByUsernameAsync(string username); 

    // Registra un nuevo usuario con sus credenciales y rol asignado
    Task<bool> SaveAsync(User user);

    // Actualiza la información del usuario (nombre, contraseña o rol)
    Task<bool> UpdateAsync(User user);

    // Elimina el acceso de un usuario al sistema
    Task<bool> DeleteAsync(int id);
}