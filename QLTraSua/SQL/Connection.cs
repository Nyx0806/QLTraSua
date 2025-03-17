using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTraSua.SQL
{
    class Connection
    {
        public static string stringConnection = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
