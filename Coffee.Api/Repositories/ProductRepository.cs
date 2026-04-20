using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbContext _context;

    public ProductRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var sql = "SELECT id as Id, categoria_id as CategoryId, nombre as Name, descripcion as Description, esta_activo as IsActive FROM Productos";
        return await _context.Connection.QueryAsync<Product>(sql);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, categoria_id as CategoryId, nombre as Name, descripcion as Description, esta_activo as IsActive FROM Productos WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(Product product)
    {
        var sql = "INSERT INTO Productos (categoria_id, nombre, descripcion, esta_activo) VALUES (@CategoryId, @Name, @Description, @IsActive)";
        var result = await _context.Connection.ExecuteAsync(sql, product);
        return result > 0;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var sql = "UPDATE Productos SET categoria_id = @CategoryId, nombre = @Name, descripcion = @Description, esta_activo = @IsActive WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, product);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Productos WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}