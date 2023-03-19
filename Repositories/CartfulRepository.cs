using System.Linq.Expressions;
using System.Data.SqlClient;
using Cartful.Service.Entities;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;

namespace Cartful.Service.Repositories
{
    public class CartfulRepository
    {

        SqlConnection _connection;

        public CartfulRepository(SqlConnection connection)
        {
            this._connection = connection;
        }

        // get entry by id
        public async Task<ActionResult<Account>> GetAsync(Account creds)
        {
            _connection.Open();

            // create a SQL command to select data from a table based on its ID
            string sql = "SELECT * FROM [dbo].[user] WHERE userName = @username AND password = @password";
            SqlCommand command = new SqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@username", creds.userName);
            command.Parameters.AddWithValue("@password", creds.password);

            // execute the command and retrieve the results
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                Account userAccount = new Account
                {
                    userId = Guid.ParseExact(reader.GetString(0), "D"),
                    firstName = reader.GetString(1),
                    lastName = reader.GetString(2),
                    userName = reader.GetString(3),
                    password = reader.GetString(4),
                    phoneNumber = reader.GetString(5)

                };

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

            string sql = "INSERT INTO [dbo].[user] (userID, firstName, lastName, userName, phoneNumber, password) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6)";
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

        //add list to database
        public async Task<IActionResult> CreateListAsync(ItemList list)
        {
            _connection.Open();

            string sql = "Insert into [dbo].list (listID, userID, title) values (@Value1, @Value2, @Value3) ";
            SqlCommand command = new SqlCommand(sql, _connection);

            // Set the values of the parameters
            command.Parameters.AddWithValue("@Value1", list.listID);
            command.Parameters.AddWithValue("@Value2", list.userID);
            command.Parameters.AddWithValue("@Value3", list.title);


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


        // need to write out these functions
        // delete list from database
        public async Task<ActionResult> DeleteListAsync(Guid listId)
        {
            await Task.Delay(1);
            return new OkResult();
        }

        // get all list belonging to a user
        public async Task<List<ItemList>> GetAllListsAsync(Guid userId)
        {
            // place holder
            await Task.Delay(1);
            return new List<ItemList>();
        }

        // add item to list
        public async Task<ActionResult> CreateAllItemsAsync(List<Item> items)
        {
            // place holder code
            await Task.Delay(1);
            return new OkResult();
        }

        // get all items from list 
        public async Task<List<Item>> GetAllItemsAsync(Guid listId)
        {
            // place holder code
            await Task.Delay(1);
            return new List<Item>();
        }
    }
}