using Dapper;
using Coffee.Core.Entities;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.DataAccess.Interfaces;

namespace Coffee.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContext _context;
    public UserRepository(IDbContext context) => _context = context;

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = "SELECT id as Id, rol_id as RoleId, username as Username, password_hash as PasswordHash, nombre_completo as FullName FROM Usuarios";
        return await _context.Connection.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var sql = "SELECT id as Id, rol_id as RoleId, username as Username, password_hash as PasswordHash, nombre_completo as FullName FROM Usuarios WHERE username = @Username";
        return await _context.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<bool> SaveAsync(User user)
    {
        var sql = "INSERT INTO Usuarios (rol_id, username, password_hash, nombre_completo) VALUES (@RoleId, @Username, @PasswordHash, @FullName)";
        var result = await _context.Connection.ExecuteAsync(sql, user);
        return result > 0;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var sql = "SELECT id as Id, rol_id as RoleId, username as Username, password_hash as PasswordHash, nombre_completo as FullName FROM Usuarios WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var sql = "UPDATE Usuarios SET rol_id = @RoleId, username = @Username, password_hash = @PasswordHash, nombre_completo = @FullName WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, user);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Usuarios WHERE id = @Id";
        var result = await _context.Connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}