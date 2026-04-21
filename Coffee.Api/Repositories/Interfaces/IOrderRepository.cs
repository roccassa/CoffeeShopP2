using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IOrderRepository {
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<int> SaveAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
}