using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface ICategoryServices
{
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(int id);
    Task<Response<CategoryDto>> SaveAsync(CategoryDto dto);
    Task<Response<CategoryDto>> UpdateAsync(CategoryDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}