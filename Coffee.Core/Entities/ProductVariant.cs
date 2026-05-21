namespace Coffee.Core.Entities;

public class ProductVariant: BaseEntity
{
    
    public int ProductId { get; set; }
    public string Size { get; set; } = string.Empty; 
    public decimal Price { get; set; }
    
    public string? ProductName { get; set; }
}