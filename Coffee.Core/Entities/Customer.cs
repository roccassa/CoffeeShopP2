namespace Coffee.Core.Entities;

public class Customer: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int LoyaltyPoints { get; set; }
}