using Microsoft.Data.SqlClient;
using System.Data;

namespace FastCore.Repositories.Infrastructure.DbFactory
{
    public interface IDatabaseFactory
    {
        IDbConnection CreateConnection();
        IDbConnection CreateConnection(string connectionString);
        IDbConnection CreateConnection(string connectionString, SqlCredential credentialSql);
    }
}
