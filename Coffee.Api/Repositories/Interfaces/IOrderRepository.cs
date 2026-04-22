using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IOrderRepository {
// Obtiene el historial completo de todas las órdenes generadas
    Task<IEnumerable<Order>> GetAllAsync();

    // Consulta la información de una orden específica mediante su ID
    Task<Order?> GetByIdAsync(int id);

    // Registra una nueva orden y devuelve el ID generado por la base de datos
    Task<int> SaveAsync(Order order);

    // Actualiza los datos de una orden existente (como el estado o el total)
    Task<bool> UpdateAsync(Order order);

    // Elimina el registro de una orden del sistema
    Task<bool> DeleteAsync(int id);
}