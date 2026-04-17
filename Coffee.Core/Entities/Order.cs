namespace Coffee.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? CustomerId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";
}