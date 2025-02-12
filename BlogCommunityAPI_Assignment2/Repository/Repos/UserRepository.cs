using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Dapper;
using System.Data;

namespace BlogCommunityAPI_Assignment2.Repository.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly IBlogCommunity _dbContext;

        public UserRepository(IBlogCommunity dbContext)
        {
            _dbContext = dbContext;
        }

        // Create a new user and return the generated UserId
        public int CreateUser(string username, string password, string email)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                // Create a DynamicParameters object to pass parameters to the stored procedure

                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);
                parameters.Add("@PasswordHash", password);
                parameters.Add("@Email", email);
                parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output); // For output parameter

                connection.Execute("dbo.CreateUser", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@UserId"); // Retrieve the output parameter value
            }
        }


        public User? GetUserByUsername(string username)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                // check the input username
                Console.WriteLine($"Input Username: {username}");

                // Execute the stored procedure
                var user = connection.QueryFirstOrDefault<User>(
                    "dbo.GetUserByUsername",
                    new { Username = username.Trim() }, // Trim the input
                    commandType: System.Data.CommandType.StoredProcedure
                );

                // check if the user was found or not
                Console.WriteLine(user != null ? "User found." : "User not found.");
                return user;
            }
        }

        public int UpdateUser(int userId, UpdateUserDTO userDTO)

        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Username", userDTO.Username, DbType.String, ParameterDirection.Input);
                parameters.Add("@PasswordHash", userDTO.PasswordHash, DbType.String, ParameterDirection.Input);
                parameters.Add("@Email", userDTO.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); // Return value

                connection.Execute("dbo.UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                // Läs av returnvärdet
                int rowsAffected = parameters.Get<int>("@Result");

                return rowsAffected;
            }
        }

        public int DeleteUser(int userId)
        {
            using (var connection = _dbContext.GetConnection()) // Get a database connection
            {
                connection.Open(); // Open the connection

                // Call the stored procedure
                return connection.Execute(
                    "dbo.DeleteUser", // Stored procedure name
                    new
                    {
                        UserId = userId // Pass UserId
                    },
                    commandType: CommandType.StoredProcedure // Specify that it's a stored procedure
                );
            }
        }



    }
}
