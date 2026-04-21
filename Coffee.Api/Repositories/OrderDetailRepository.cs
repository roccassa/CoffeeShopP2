using Coffee.Api.DataAccess.Interfaces;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;
using Dapper;

namespace Coffee.Api.Repositories;

public class OrderDetailRepository : IOrderDetailRepository {
    private readonly IDbContext _context;
    public OrderDetailRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId) {
        var sql = "SELECT id as Id, orden_id as OrderId, presentacion_id as ProductVariantId, cantidad as Quantity, precio_unitario as UnitPrice FROM DetalleOrden WHERE orden_id = @OrderId";
        return await _context.Connection.QueryAsync<OrderDetail>(sql, new { OrderId = orderId });
    }

    public async Task<bool> SaveAsync(OrderDetail detail) {
        var sql = "INSERT INTO DetalleOrden (orden_id, presentacion_id, cantidad, precio_unitario) VALUES (@OrderId, @ProductVariantId, @Quantity, @UnitPrice)";
        var result = await _context.Connection.ExecuteAsync(sql, detail);
        return result > 0;
    }
    
    public async Task<IEnumerable<OrderDetail>> GetAllAsync() {
        var sql = "SELECT id as Id, orden_id as OrderId, presentacion_id as ProductVariantId, cantidad as Quantity, precio_unitario as UnitPrice FROM DetalleOrden";
        return await _context.Connection.QueryAsync<OrderDetail>(sql);
    }

    public async Task<OrderDetail?> GetByIdAsync(int id) {
        var sql = "SELECT id as Id, orden_id as OrderId, presentacion_id as ProductVariantId, cantidad as Quantity, precio_unitario as UnitPrice FROM DetalleOrden WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<OrderDetail>(sql, new { Id = id });
    }

    public async Task<bool> UpdateAsync(OrderDetail detail) {
        var sql = "UPDATE DetalleOrden SET orden_id = @OrderId, presentacion_id = @ProductVariantId, cantidad = @Quantity, precio_unitario = @UnitPrice WHERE id = @Id";
        return await _context.Connection.ExecuteAsync(sql, detail) > 0;
    }

    public async Task<bool> DeleteAsync(int id) {
        var sql = "DELETE FROM DetalleOrden WHERE id = @Id";
        return await _context.Connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}