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
        public async Task<ActionResult<Account>> GetUserAsync(Account creds)
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

        public async Task<IActionResult> CreateUserAsync(Account account)
        {
            _connection.Open();

            string sql = "INSERT INTO [dbo].[user] (userID, firstName, lastName, userName, password, phoneNumber) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6)";
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

        public async Task<ActionResult> DeleteListAsync(Guid listId)
        {
            _connection.Open();

            string sql = "DELETE FROM [dbo].list WHERE listID=(@Value1)";
            SqlCommand command = new SqlCommand(sql, _connection);

            // Set the values of the parameters
            command.Parameters.AddWithValue("@Value1", listId);


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

        // get all list belonging to a user
        public async Task<List<ItemList>> GetAllListsAsync(Guid userId)
        {
            try
            {
                _connection.Open();

                string sql = "SELECT * FROM [dbo].list WHERE UserID = @UserId";
                SqlCommand command = new SqlCommand(sql, _connection);

                // Set the values of the parameters
                command.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                List<ItemList> itemLists = new List<ItemList>();

                while (await reader.ReadAsync())
                {
                    ItemList itemList = new ItemList()
                    {
                        listID = Guid.ParseExact(reader.GetString(0), "D"),
                        userID = Guid.ParseExact(reader.GetString(1), "D"),
                        title = reader.GetString(2),
                        items = new List<Item>()
                    };
                    itemLists.Add(itemList);
                }

                reader.Close();
                _connection.Close();

                foreach (ItemList itemList in itemLists)
                {
                    itemList.items = await GetAllItemsAsync(itemList.listID);
                }

                return itemLists;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");

                // Return an error message to the user or take some other appropriate action
                throw new Exception("An error occurred while getting all lists.");
            }
        }


        // add item to list
        public async Task<IActionResult> CreateAllItemsAsync(List<Item> items)
        {
            try
            {
                _connection.Open();

                string sql = "INSERT INTO [dbo].item (itemID, listID, itemName, isChecked) VALUES (@ItemID, @ListID, @ItemName, @IsChecked)";
                SqlCommand command = new SqlCommand(sql, _connection);

                foreach (Item item in items)
                {
                    // Set the values of the parameters for each item in the list
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemID", item.itemID);
                    command.Parameters.AddWithValue("@ListID", item.listID);
                    command.Parameters.AddWithValue("@ItemName", item.itemName);
                    command.Parameters.AddWithValue("@IsChecked", item.isChecked);

                    // Execute the command for each item in the list
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected <= 0)
                    {
                        return new BadRequestResult();
                    }
                }

                return new OkResult();
            }
            finally
            {
                _connection.Close();
            }
        }

        // get all items from list 
        public async Task<List<Item>> GetAllItemsAsync(Guid listID)
        {
            try
            {
                _connection.Open();

                string sql = "SELECT * FROM [dbo].item WHERE ListID = @ListId";
                SqlCommand command = new SqlCommand(sql, _connection);

                // Set the values of the parameters
                command.Parameters.AddWithValue("@ListId", listID);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                List<Item> items = new List<Item>();

                while (await reader.ReadAsync())
                {
                    Item item = new Item()
                    {
                        itemID = Guid.ParseExact(reader.GetString(0), "D"),
                        listID = Guid.ParseExact(reader.GetString(1), "D"),
                        itemName = reader.GetString(2),
                        isChecked = reader.GetByte(3) == 1 ? true : false
                    };

                    items.Add(item);
                }

                reader.Close();

                return items;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ActionResult> DeleteItemAsync(Guid itemID)
        {
            _connection.Open();

            string sql = "DELETE FROM [dbo].item WHERE itemID=(@Value1)";
            SqlCommand command = new SqlCommand(sql, _connection);

            // Set the values of the parameters
            command.Parameters.AddWithValue("@Value1", itemID);


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

