using Coffee.Core.Dto;
using Coffee.Core.Entities;

namespace Coffee.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<bool> ProductCategoryExists(int id);
    
    // Método para guardar la categoría
    Task<CategoryDto> SaveAsync(CategoryDto categoryDto);

    // Método para actualizar la categoría
    Task<CategoryDto> UpdateAsync(CategoryDto categoryDto);

    // Método para retornar una lista
    Task<List<CategoryDto>> GetAllAsync();

    // Método para borrar una categoría por id
    Task<bool> DeleteAsync(int id);

    // Método para obtener una categoría por id
    Task<CategoryDto> GetByIdAsync(int id);
}