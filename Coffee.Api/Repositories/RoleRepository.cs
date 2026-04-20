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
}