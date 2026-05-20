using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IProductService
{
    Task<Response<List<ProductDto>>> GetAllAsync();
    Task<Response<ProductDto>> GetByIdAsync(int id);
    Task<Response<ProductDto>> CreateAsync(ProductDto dto);
    Task<Response<ProductDto>> UpdateAsync(ProductDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
