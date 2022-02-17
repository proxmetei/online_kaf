using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChat.Models
{
	public class SqlDbConnectionFactory: IDbConnectionFactory
	{
        private string _connectionString;

        public SqlDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString);
        }
    }
}
