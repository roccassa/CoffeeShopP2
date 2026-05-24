namespace Coffee.Core.Entities;

public class Order:BaseEntity
{
    
    public int UserId { get; set; }
    public int? CustomerId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pendiente";
    
    public string? UserName { get; set; }

    public string? CustomerName { get; set; }

    public string? PaymentMethodName { get; set; }
}