using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface ICustomerRepository
{
    // Obtiene el listado completo de clientes registrados
    Task<IEnumerable<Customer>> GetAllAsync();

    // Busca un cliente específico por su ID (puede retornar nulo si no existe)
    Task<Customer?> GetByIdAsync(int id);

    // Registra un nuevo cliente en la base de datos
    Task<bool> SaveAsync(Customer customer);

    // Actualiza la información de un cliente existente
    Task<bool> UpdateAsync(Customer customer);

    // Realiza la eliminación de un registro de cliente por su ID
    Task<bool> DeleteAsync(int id);
}