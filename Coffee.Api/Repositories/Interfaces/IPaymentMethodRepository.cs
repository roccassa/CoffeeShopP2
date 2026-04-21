using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(int id);
    Task<bool> SaveAsync(PaymentMethod method);
    Task<bool> UpdateAsync(PaymentMethod method);
    Task<bool> DeleteAsync(int id);
}