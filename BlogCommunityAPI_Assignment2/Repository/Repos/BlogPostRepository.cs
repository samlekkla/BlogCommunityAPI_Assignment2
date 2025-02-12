using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Dapper;
using System.Data;

namespace BlogCommunityAPI_Assignment2.Repository.Repos
{

    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IBlogCommunity _dbContext;


        public BlogPostRepository(IBlogCommunity dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateBlogPost(BlogPostDTO post, int loggedInUserID)
        {
            using (var connection = _dbContext.GetConnection()) // Öppna databasanslutning

            {
                connection.Open();

                // 🔍 Hämta CategoryName baserat på CategoryID
                var category = connection.QueryFirstOrDefault<Category>(
                    "SELECT CategoryID, CategoryName FROM Categories WHERE CategoryID = @CategoryID",
                    new { post.CategoryID });

                if (category == null)
                {
                    throw new Exception("Ogiltigt CategoryID. Kategorin finns inte.");
                }

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", loggedInUserID);
                parameters.Add("@PostID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@CategoryID", category.CategoryID);
                parameters.Add("@Title", post.Title);
                parameters.Add("@Content", post.Content);

                // 🔹 Anropa den lagrade proceduren
                connection.Execute("CreateBlogPost", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@PostID");
            }
        }



        public IEnumerable<BlogPostDTO> GetAllBlogPosts()
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                return connection.Query<BlogPostDTO>("GetBlogPosts", commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                return connection.Query<CategoryDTO>(
                    "GetAllCategories",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public IEnumerable<BlogPostDTO> GetBlogPostsByUser(int userId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                return connection.Query<BlogPostDTO>("GetBlogPostsByUser",
                    new { UserID = userId },
                    commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public bool UpdateBlogPost(int postId, BlogPostDTO post, int userId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                var parameters = new
                {
                    PostID = postId,
                    UserID = userId, // Använd userId som är kopplat till den inloggade användaren
                    Title = post.Title,
                    Content = post.Content,
                    CategoryID = post.CategoryID
                };

                var rowsAffected = connection.Execute("UpdateBlogPost", parameters, commandType: CommandType.StoredProcedure);

                return rowsAffected > 0; // Returnera true om uppdatering lyckades
            }
        }

        public bool DeleteBlogPost(int postId, int userId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                var parameters = new
                {
                    PostID = postId,
                    UserID = userId // Använd userId som är kopplat till den inloggade användaren
                };

                var rowsAffected = connection.Execute("DeleteBlogPost", parameters, commandType: CommandType.StoredProcedure);

                return rowsAffected > 0; // Returnera true om borttagning lyckades
            }
        }

        public BlogPostDTO GetBlogPostById(int postId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                return connection.QueryFirstOrDefault<BlogPostDTO>(
                    "GetBlogPostById",
                    new { PostID = postId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public IEnumerable<BlogPostDTO> SearchPostsByTitle(string title)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                return connection.Query<BlogPostDTO>(
                    "SearchPostsByTitle",
                    new { Title = $"%{title}%" }, // Använder LIKE-sökning
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public IEnumerable<BlogPostDTO> SearchPostsByCategory(int categoryId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                return connection.Query<BlogPostDTO>(
                    "SearchPostsByCategory",
                    new { CategoryID = categoryId },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
    }

}
