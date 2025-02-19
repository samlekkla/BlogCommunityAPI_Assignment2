using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Dapper;
using System.Data;
using System.Data.Common;

namespace BlogCommunityAPI_Assignment2.Repository.Repos
{

    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IBlogCommunity _dbContext;


        public BlogPostRepository(IBlogCommunity dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateBlogPost(BlogPostDTO post)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                // 🔍 Hämta CategoryName baserat på CategoryID
                var categoryName = connection.QueryFirstOrDefault<string>(
                    "SELECT CategoryName FROM Categories WHERE CategoryID = @CategoryID",
                    new { post.CategoryID });

                if (string.IsNullOrEmpty(categoryName))
                {
                    throw new Exception("Invalid CategoryID. Category does not exist.");
                }

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", post.UserID);
                parameters.Add("@CategoryID", post.CategoryID);
                parameters.Add("@CategoryName", categoryName);
                parameters.Add("@Title", post.Title);
                parameters.Add("@Content", post.Content);
                parameters.Add("@PostID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // 🔹 Anropa lagrad procedur
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

        public bool UpdateBlogPost(int postId, UpdateBlogPostDTO post, int userId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();

                var parameters = new
                {
                    PostID = postId,
                    UserID = userId, // Kontrollera att användaren äger inlägget
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
                    UserID = userId // Säkerställer att endast inläggets ägare kan radera det
                };

                var rowsAffected = connection.Execute("DeleteBlogPost", parameters, commandType: CommandType.StoredProcedure);

                return rowsAffected > 0; // Returnera true om raderingen lyckades
            }
        }

        public int GetPostOwnerId(int postId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                return connection.QuerySingleOrDefault<int>(
                    "GetPostOwnerId",
                    new { PostID = postId },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public BlogPostDTO GetBlogPostById(int postId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                return connection.QuerySingleOrDefault<BlogPostDTO>(
                    "GetBlogPostById",
                    new { PostID = postId },
                    commandType: CommandType.StoredProcedure);
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
