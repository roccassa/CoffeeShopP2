namespace Coffee.Core.Entities;

public class Product: BaseEntity
{
    //public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}