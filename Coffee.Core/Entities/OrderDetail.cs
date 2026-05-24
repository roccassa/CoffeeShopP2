namespace Coffee.Core.Entities;

public class OrderDetail:BaseEntity
{
    public int OrderId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    public string? PresentationName { get; set; }
    public string? ProductName { get; set; }
}