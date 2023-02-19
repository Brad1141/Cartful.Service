using System.Linq.Expressions;
using System.Data.SqlClient;
using Cartful.Service.Entities;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Cartful.Service.Repositories
{
    public class AccountRepository
    {

        SqlConnection _connection;

        public AccountRepository(SqlConnection connection)
        {
            this._connection = connection;
        }

        // get entry by id
        public async Task<ActionResult<Account>> GetAsync(Guid id)
        {
            _connection.Open();

            // create a SQL command to select data from a table based on its ID
            string sql = "SELECT * FROM MyTable WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@Id", id);

            // execute the command and retrieve the results
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                Account userAccount = new Account();
                userAccount.firstName = reader.GetString(1);
                userAccount.lastName = reader.GetString(2);
                userAccount.userName = reader.GetString(3);
                userAccount.password = reader.GetString(4);
                userAccount.phoneNumber = reader.GetString(5);

                reader.Close();
                _connection.Close();

                return userAccount;
            }
            else
            {
                return new NotFoundResult();
            }

        }

        // add item to db
        public async Task<IActionResult> CreateAsync(Account account)
        {
            _connection.Open();
            
            string sql = "INSERT INTO user (userID, firstName, lastName, userName, phoneNumber, password) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6)";
            SqlCommand command = new SqlCommand(sql, _connection);

            // Set the values of the parameters
            command.Parameters.AddWithValue("@Value1", account.userId);
            command.Parameters.AddWithValue("@Value2", account.firstName);
            command.Parameters.AddWithValue("@Value3", account.lastName);
            command.Parameters.AddWithValue("@Value4", account.userName);
            command.Parameters.AddWithValue("@Value5", account.password);
            command.Parameters.AddWithValue("@Value6", account.phoneNumber);

            // Execute the command
            int rowsAffected = await command.ExecuteNonQueryAsync();
            _connection.Close();

            if (rowsAffected <= 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkResult();
            }
        }
    }
}