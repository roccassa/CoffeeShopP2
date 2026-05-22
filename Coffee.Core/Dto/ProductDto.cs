using Coffee.Core.Entities;

namespace Coffee.Core.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
}