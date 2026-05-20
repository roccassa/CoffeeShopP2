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
        var sql = "SELECT id as Id, nombre as Name, descripcion as Description FROM Categorias";
        return await _context.Connection.QueryAsync<Category>(sql);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, nombre as Name, descripcion as Description FROM Categorias WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
    }

    public async Task<Category> SaveAsync(Category category)
    {
        // Inserta en tus columnas en minúscula y recupera el AUTO_INCREMENT usando LAST_INSERT_ID()
        var sql = @"INSERT INTO Categorias (nombre, descripcion) VALUES (@Name, @Description);
                    SELECT LAST_INSERT_ID();"; 
        
        category.Id = await _context.Connection.QuerySingleAsync<int>(sql, new { 
            Name = category.Name, 
            Description = category.Description 
        });
        
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        var sql = "UPDATE Categorias SET nombre = @Name, descripcion = @Description WHERE id = @Id";
        await _context.Connection.ExecuteAsync(sql, category);
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Categorias WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}