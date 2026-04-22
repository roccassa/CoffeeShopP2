using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IProductVariantRepository
{
// Recupera todas las variantes de productos (tamaños/precios) registradas
    Task<IEnumerable<ProductVariant>> GetAllAsync();

    // Busca una variante específica (ej. un tamaño particular) por su ID
    Task<ProductVariant?> GetByIdAsync(int id);

    // Registra una nueva variante vinculada a un producto existente
    Task<bool> SaveAsync(ProductVariant variant);

    // Actualiza los datos de una variante, como el cambio de precio o tamaño
    Task<bool> UpdateAsync(ProductVariant variant);

    // Elimina una variante de producto del sistema
    Task<bool> DeleteAsync(int id);
}