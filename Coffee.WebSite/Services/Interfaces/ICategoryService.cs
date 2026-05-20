using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface ICategoryService
{
 
    
    // Quitamos los Response<> y dejamos los tipos directos que manda la API
    Task<List<CategoryDto>> GetAllAsync();
    
    Task<CategoryDto> GetByIdAsync(int id);
    
    Task<CategoryDto> CreateAsync(CategoryDto categoryDto);
    
    Task<CategoryDto> UpdateAsync(CategoryDto categoryDto);
    
    Task<bool> DeleteAsync(int id);
}