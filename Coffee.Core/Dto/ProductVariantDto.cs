namespace Coffee.Core.Dto;

public class ProductVariantDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public string? ProductName { get; set; }
}