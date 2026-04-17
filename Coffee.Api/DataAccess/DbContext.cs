using MySql.Data.MySqlClient;
using System.Data;
using Coffee.Api.DataAccess.Interfaces;
namespace Coffee.Api.DataAccess;

public class DbContext: IDbContext
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public DbContext()
    {
        _connectionString = "server=localhost;port=3306;user=root;password=12345678;database=CafeteriaDB";
    }

    public IDbConnection Connection
    {
        get
        {
          
            if (_connection == null )
            {
                _connection = new MySqlConnection(_connectionString);
                
            }

            return _connection;
        }
    }
}