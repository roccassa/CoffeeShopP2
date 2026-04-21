using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IProductVariantRepository
{
    Task<IEnumerable<ProductVariant>> GetAllAsync();
    Task<ProductVariant?> GetByIdAsync(int id);
    Task<bool> SaveAsync(ProductVariant variant);
    Task<bool> UpdateAsync(ProductVariant variant);
    Task<bool> DeleteAsync(int id);
}