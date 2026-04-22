using Coffee.Core.Entities;
namespace Coffee.Api.Repositories.Interfaces;

public interface IProductRepository
{
// Recupera la lista completa de productos registrados en el catálogo
    Task<IEnumerable<Product>> GetAllAsync();

    // Obtiene la información detallada de un producto específico mediante su ID
    Task<Product> GetByIdAsync(int id);

    // Registra un nuevo producto (ej. un nuevo tipo de café) en la base de datos
    Task<bool> SaveAsync(Product product);

    // Actualiza los datos de un producto existente (nombre, descripción o estado)
    Task<bool> UpdateAsync(Product product);

    // Realiza el borrado del registro de un producto del sistema
    Task<bool> DeleteAsync(int id);
}