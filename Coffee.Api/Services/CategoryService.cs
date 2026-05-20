using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.Services.Interfaces;
using Coffee.Core.Dto;
using Coffee.Core.Entities;

namespace Coffee.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> ProductCategoryExists(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return (category != null);
    }

    public async Task<CategoryDto> SaveAsync(CategoryDto categoryDto)
    {
        // Mapeo manual de DTO a Entidad de dominio
        var category = new Category()
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description
            // Aquí puedes mapear campos de auditoría si tu entidad los posee
        };

        category = await _categoryRepository.SaveAsync(category);

        // Sincronizamos el ID generado en la entidad de vuelta al DTO que se va a retornar
        categoryDto.Id = category.Id;

        return categoryDto;
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);

        if (category == null)
        {
            throw new Exception($"Category with ID {categoryDto.Id} not found or was deleted");
        }

        // Actualización de propiedades de la entidad
        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;

        await _categoryRepository.UpdateAsync(category);

        // Retornamos el DTO actualizado utilizando el constructor de mapeo
        return new CategoryDto(category);
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        // Conversión explícita y mapeo de la lista mediante expresiones lambda
        var categoriesDto = categories.Select(c => new CategoryDto(c)).ToList();
        return categoriesDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Delega directamente la baja lógica al repositorio
        return await _categoryRepository.DeleteAsync(id);
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new Exception("Product category not found");

        return new CategoryDto(category);
    }
}