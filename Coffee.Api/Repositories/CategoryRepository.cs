using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Coffee.Api.Repositories;

public class CategoryRepository :ICategoryRepository
{
    private readonly IDbContext _context;

    public CategoryRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        // Usamos alias (AS) para que Dapper mapee 'nombre' a la propiedad 'Name' de tu entidad
        var sql = "SELECT id as Id, nombre as Name, descripcion as Description FROM Categorias";
        return await _context.Connection.QueryAsync<Category>(sql);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, nombre as Name, descripcion as Description FROM Categorias WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
    }

    public async Task<bool> SaveAsync(Category category)
    {
        var sql = "INSERT INTO Categorias (nombre, descripcion) VALUES (@Name, @Description)";
        var result = await _context.Connection.ExecuteAsync(sql, new { 
            Name = category.Name, 
            Description = category.Description 
        });
        return result > 0;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        var sql = "UPDATE Categorias SET nombre = @Name, descripcion = @Description WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, category);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Categorias WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
    
}