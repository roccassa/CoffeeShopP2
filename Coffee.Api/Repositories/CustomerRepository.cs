using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbContext _context;
    public CustomerRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var sql = "SELECT id as Id, nombre as Name, correo as Email, puntos_lealtad as LoyaltyPoints FROM Clientes";
        return await _context.Connection.QueryAsync<Customer>(sql);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, nombre as Name, correo as Email, puntos_lealtad as LoyaltyPoints FROM Clientes WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(Customer customer)
    {
        var sql = "INSERT INTO Clientes (nombre, correo, puntos_lealtad) VALUES (@Name, @Email, @LoyaltyPoints)";
        var result = await _context.Connection.ExecuteAsync(sql, customer);
        return result > 0;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var sql = "UPDATE Clientes SET nombre = @Name, correo = @Email, puntos_lealtad = @LoyaltyPoints WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Clientes WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}