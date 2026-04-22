  
using Coffee.Core.Entities;
namespace Coffee.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    // Método para guardar la categoría
    Task<bool> SaveAsync(Category category);

    // Método para actualizar la categoría
    Task<bool> UpdateAsync(Category category);

    // Método para retornar una lista
    Task<IEnumerable<Category>> GetAllAsync();

    // Método para borrar una categoría por id
    Task<bool> DeleteAsync(int id);

    // Método para obtener una categoría por id
    Task<Category> GetByIdAsync(int id);
    
}