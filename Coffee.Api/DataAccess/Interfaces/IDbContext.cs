using System.Data;
namespace Coffee.Api.DataAccess.Interfaces;

public interface IDbContext
{
    IDbConnection Connection { get; }
}