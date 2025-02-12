using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Entities;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
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
                    new { PostId = postId, UserId = userId, CommentText = commentText },
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
                    new { PostId = postId },
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
                    "SELECT UserID FROM Posts WHERE PostID = @PostID",
                    new { PostID = postId });

                if (postOwnerId == null)
                {
                    throw new ArgumentException("Post not found.");
                }
                return postOwnerId.Value;
            }
        }
    }

}
