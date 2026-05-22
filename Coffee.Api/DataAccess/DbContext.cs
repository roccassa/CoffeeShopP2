using MySql.Data.MySqlClient;
using System.Data;
using Coffee.Api.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Coffee.Api.DataAccess;

public class DbContext : IDbContext
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró 'DefaultConnection' en appsettings.json");
    }

    public IDbConnection Connection
    {
        get
        {
            if (_connection == null)
                _connection = new MySqlConnection(_connectionString);

            return _connection;
        }
    }
}