using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IOrderDetailRepository {
    Task<IEnumerable<OrderDetail>> GetAllAsync();
    Task<OrderDetail?> GetByIdAsync(int id);
    Task<bool> SaveAsync(OrderDetail detail);
    Task<bool> UpdateAsync(OrderDetail detail);
    Task<bool> DeleteAsync(int id);
}