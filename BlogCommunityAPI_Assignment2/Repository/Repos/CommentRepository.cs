using BlogCommunityAPI_Assignment2.Repository.Entities;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Dapper;
using System.Data;

namespace BlogCommunityAPI_Assignment2.Repository.Repos
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IBlogCommunity _dbContext;

        public CommentRepository(IBlogCommunity dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddComment(int postId, int userId, string commentText)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                connection.Execute("AddComment",
                    new { PostID = postId, UserID = userId, CommentText = commentText },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                return connection.Query<Comment>(
                    "GetCommentsByPost",
                    new { PostID = postId },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public int GetPostOwnerId(int postId)
        {
            using (var connection = _dbContext.GetConnection())
            {
                connection.Open();
                var postOwnerId = connection.QuerySingleOrDefault<int?>(
                    "SELECT UserID FROM BlogPosts WHERE PostID = @PostID",
                    new { PostID = postId });

                return postOwnerId ?? -1;  // Returnera -1 om inlägget inte finns
            }
        }
    }
}
