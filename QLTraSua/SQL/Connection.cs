using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTraSua.SQL
{
    class Connection
    {
        public static string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\C_C#_C++\Visual Studio Code\QLTraSua\QLTraSua\Database\Trasua.mdf"";Integrated Security=True";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
