using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Repository.Helper
{
    public class DbConnectionHelper
    {
        public string ConnectionString { get; }

        public DbConnectionHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
