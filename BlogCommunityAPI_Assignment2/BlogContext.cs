using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace BlogCommunityAPI_Assignment2
{
    public class BlogContext: IBlogCommunity
    {
        private readonly string _connString;

        public BlogContext()
        {
            _connString = string.Empty;
        }

        public BlogContext(IConfiguration config)
        {
            _connString = config.GetConnectionString("Blog")
            ?? throw new InvalidOperationException("Connection string 'BlogCommunity' not found.");
        }

        public SqlConnection GetConnection()
        {


            if (string.IsNullOrEmpty(_connString))
            {
                Console.WriteLine("Connection string is not initialized.");
                throw new InvalidOperationException("The connection string has not been initialized.");
            }

            try
            {
                var connection = new SqlConnection(_connString);
                Console.WriteLine("Successfully created a database connection.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating connection: " + ex.Message);
                throw;
            }
        }
    }
}
