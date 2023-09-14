using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BuildingBlocks;

public class  SqlConnectionFactory: ISqlconnectionfactory
{
    private readonly  string _connectionstring;

    public SqlConnectionFactory(IConfiguration conf)
    {
       _connectionstring = conf.GetConnectionString("DefaultConnection") ??
                            throw new ApplicationException("connection string is missing");
    }
    
    public   IDbConnection Create()
    {
        return new NpgsqlConnection(_connectionstring);
    }

    
}
public interface ISqlconnectionfactory
{
    public  IDbConnection Create();
}