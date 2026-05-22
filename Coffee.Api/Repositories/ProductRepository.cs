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
        // NOTE: Run this migration once if the column doesn't exist:
        // ALTER TABLE Productos ADD COLUMN imagen_url VARCHAR(500) NULL;
        var sql = @"
        SELECT
            p.id          AS Id,
            p.categoria_id AS CategoryId,
            c.nombre      AS CategoryName,
            p.nombre      AS Name,
            p.descripcion AS Description,
            p.esta_activo AS IsActive,
            COALESCE(p.imagen_url, '') AS ImageUrl
        FROM Productos p
        INNER JOIN Categorias c ON p.categoria_id = c.id";

        return await _context.Connection.QueryAsync<Product>(sql);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = @"SELECT id AS Id, categoria_id AS CategoryId, nombre AS Name,
                    descripcion AS Description, esta_activo AS IsActive,
                    COALESCE(imagen_url, '') AS ImageUrl
                    FROM Productos WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(Product product)
    {
        var insertSql = @"INSERT INTO Productos (categoria_id, nombre, descripcion, esta_activo, imagen_url)
                          VALUES (@CategoryId, @Name, @Description, @IsActive, @ImageUrl)";

        // Keep the connection open across both calls so LAST_INSERT_ID()
        // is guaranteed to belong to this session (not another pool connection).
        bool wasOpen = _context.Connection.State == System.Data.ConnectionState.Open;
        if (!wasOpen) _context.Connection.Open();

        try
        {
            await _context.Connection.ExecuteAsync(insertSql, product);
            product.Id = await _context.Connection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()");
            return product.Id > 0;
        }
        finally
        {
            if (!wasOpen) _context.Connection.Close();
        }
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var sql = @"UPDATE Productos SET categoria_id=@CategoryId, nombre=@Name,
                    descripcion=@Description, esta_activo=@IsActive, imagen_url=@ImageUrl
                    WHERE id=@Id";
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
