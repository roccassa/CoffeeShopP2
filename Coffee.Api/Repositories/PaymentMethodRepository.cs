using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly IDbContext _context;
    public PaymentMethodRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        var sql = "SELECT id as Id, nombre as Name FROM MetodosPago";
        return await _context.Connection.QueryAsync<PaymentMethod>(sql);
    }

    public async Task<PaymentMethod?> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, nombre as Name FROM MetodosPago WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<PaymentMethod>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(PaymentMethod method)
    {
        var sql = "INSERT INTO MetodosPago (nombre) VALUES (@Name)";
        var result = await _context.Connection.ExecuteAsync(sql, new { Name = method.Name });
        return result > 0;
    }

    public async Task<bool> UpdateAsync(PaymentMethod method)
    {
        var sql = "UPDATE MetodosPago SET nombre = @Name WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Name = method.Name, Id = method.Id });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM MetodosPago WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}