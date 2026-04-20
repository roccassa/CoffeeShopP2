using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IDbContext _context;
    public RoleRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var sql = "SELECT id as Id, nombre as Name FROM Roles";
        return await _context.Connection.QueryAsync<Role>(sql);
    }

    public async Task<bool> SaveAsync(Role role)
    {
        var sql = "INSERT INTO Roles (nombre) VALUES (@Name)";
        var result = await _context.Connection.ExecuteAsync(sql, new { Name = role.Name });
        return result > 0;
    }
    
    public async Task<Role> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, nombre as Name FROM Roles WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id });
    }
    
    public async Task<bool> UpdateAsync(Role role)
    {
        var sql = "UPDATE Roles SET nombre = @Name WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Name = role.Name, Id = role.Id });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Roles WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}