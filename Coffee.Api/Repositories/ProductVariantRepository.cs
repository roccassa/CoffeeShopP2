using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class ProductVariantRepository : IProductVariantRepository
{
    private readonly IDbContext _context;
    public ProductVariantRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<ProductVariant>> GetAllAsync()
    {
        var sql = "SELECT id as Id, producto_id as ProductId, tamano as Size, precio as Price FROM Presentaciones";
        return await _context.Connection.QueryAsync<ProductVariant>(sql);
    }

    public async Task<ProductVariant?> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, producto_id as ProductId, tamano as Size, precio as Price FROM Presentaciones WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<ProductVariant>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(ProductVariant variant)
    {
        var sql = "INSERT INTO Presentaciones (producto_id, tamano, precio) VALUES (@ProductId, @Size, @Price)";
        var result = await _context.Connection.ExecuteAsync(sql, variant);
        return result > 0;
    }

    public async Task<bool> UpdateAsync(ProductVariant variant)
    {
        var sql = "UPDATE Presentaciones SET producto_id = @ProductId, tamano = @Size, precio = @Price WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, variant);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Presentaciones WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}