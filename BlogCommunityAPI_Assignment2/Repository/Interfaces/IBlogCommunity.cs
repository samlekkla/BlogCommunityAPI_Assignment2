using Microsoft.Data.SqlClient;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface IBlogCommunity
    {
        public SqlConnection GetConnection();
    }
}
