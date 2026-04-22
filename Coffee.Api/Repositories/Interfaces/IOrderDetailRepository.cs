using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IOrderDetailRepository {
    // Recupera todos los detalles de productos asociados a las órdenes
    Task<IEnumerable<OrderDetail>> GetAllAsync();

    // Obtiene un detalle específico mediante su identificador único
    Task<OrderDetail?> GetByIdAsync(int id);

    // Guarda el desglose de un producto específico dentro de una orden
    Task<bool> SaveAsync(OrderDetail detail);

    // Actualiza la cantidad o el precio de un detalle de orden existente
    Task<bool> UpdateAsync(OrderDetail detail);

    // Elimina el registro de un detalle de orden del sistema
    Task<bool> DeleteAsync(int id);
}