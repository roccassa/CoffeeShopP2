using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IProductVariantService
{
    Task<Response<List<ProductVariantDto>>> GetAllAsync();
    Task<Response<ProductVariantDto>> GetByIdAsync(int id);
    Task<Response<ProductVariantDto>> CreateAsync(ProductVariantDto dto);
    Task<Response<ProductVariantDto>> UpdateAsync(ProductVariantDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
