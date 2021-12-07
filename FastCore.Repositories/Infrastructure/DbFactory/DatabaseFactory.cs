using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace FastCore.Repositories.Infrastructure.DbFactory
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IConfiguration _configuration;
        private string _defaultConnection;
        public DatabaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultConnection = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
        {
            try
            {
                var connection = new SqlConnection(_defaultConnection);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IDbConnection CreateConnection(string connectionString)
        {
            try
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IDbConnection CreateConnection(string connectionString, SqlCredential credentialSql)
        {
            try
            {
                var connection = new SqlConnection(connectionString, credentialSql);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
