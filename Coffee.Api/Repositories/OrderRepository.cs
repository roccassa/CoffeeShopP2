using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class OrderRepository : IOrderRepository {
    private readonly IDbContext _context;
    public OrderRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<Order>> GetAllAsync() {
        var sql = "SELECT id as Id, usuario_id as UserId, cliente_id as CustomerId, metodo_pago_id as PaymentMethodId, fecha as Date, total as Total, estado as Status FROM Ordenes";
        return await _context.Connection.QueryAsync<Order>(sql);
    }

    public async Task<int> SaveAsync(Order order) {
        var sql = @"INSERT INTO Ordenes (usuario_id, cliente_id, metodo_pago_id, total, estado) 
                    VALUES (@UserId, @CustomerId, @PaymentMethodId, @Total, @Status);
                    SELECT LAST_INSERT_ID();";
        return await _context.Connection.ExecuteScalarAsync<int>(sql, order);
    }

    public async Task<bool> UpdateStatusAsync(int id, string status) {
        var sql = "UPDATE Ordenes SET estado = @Status WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Status = status, Id = id });
        return result > 0;
    }
    
    public async Task<Order?> GetByIdAsync(int id) {
        var sql = "SELECT id as Id, usuario_id as UserId, cliente_id as CustomerId, metodo_pago_id as PaymentMethodId, fecha as Date, total as Total, estado as Status FROM Ordenes WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
    }

    public async Task<bool> UpdateAsync(Order order) {
        var sql = "UPDATE Ordenes SET usuario_id = @UserId, cliente_id = @CustomerId, metodo_pago_id = @PaymentMethodId, total = @Total, estado = @Status WHERE id = @Id";
        return await _context.Connection.ExecuteAsync(sql, order) > 0;
    }

    public async Task<bool> DeleteAsync(int id) {
        var sql = "DELETE FROM Ordenes WHERE id = @Id";
        return await _context.Connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}