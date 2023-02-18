using System.Linq.Expressions;
using System.Data.SqlClient;
using Cartful.Service.Entities;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Cartful.Service.Repositories
{
    public class accountRepository
    {

        SqlConnection _connection;

        public accountRepository(SqlConnection connection)
        {
            this._connection = connection;
        }

        // calls to databases are often async because they take a long time to complete
        // this setup allows the program to do other stuff while this function is still running
        // get all entries in db
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
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
            SqlDataReader reader = command.ExecuteReader();

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

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        // add item to db
        public async Task<IActionResult> CreateAsync(Account account)
        {
            string sql = "INSERT INTO user (Column1, Column2, Column3, Column4, Column5, Column6) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6)";
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

            if (rowsAffected <= 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkResult();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}