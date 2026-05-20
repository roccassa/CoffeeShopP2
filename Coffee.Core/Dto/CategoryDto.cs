using Coffee.Core.Entities;

namespace Coffee.Core.Dto;

public class CategoryDto : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public CategoryDto() { }

    public CategoryDto(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
    }
}