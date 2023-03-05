using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Cartful.Service
{
    class TestSql
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Data Source=11.102.0.28;Initial Catalog=railfann_cartful;User ID=railfann_ice;Password=Z05Mdy(MPoun";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            // create a SQL command to select data from a table based on its ID
            string sql = "SELECT * FROM user";
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            // execute the command and retrieve the results
            SqlDataReader reader = await command.ExecuteReaderAsync();
            sqlConnection.Close();
            Console.WriteLine("Hello World!");
        }
    }
}
