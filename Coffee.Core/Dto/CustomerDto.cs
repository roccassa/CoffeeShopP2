using Coffee.Core.Entities;

namespace Coffee.Core.Dto;

public class CustomerDto : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int LoyaltyPoints { get; set; }

    public CustomerDto() { }

    public CustomerDto(Customer customer)
    {
        Id = customer.Id;
        Name = customer.Name;
        Email = customer.Email;
        LoyaltyPoints = customer.LoyaltyPoints;
    }
}