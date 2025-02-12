using BlogCommunityAPI_Assignment2.DTO;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface IBlogPostRepository
    {
        int CreateBlogPost(BlogPostDTO post, int loggedInUserID);
        bool UpdateBlogPost(int postId, BlogPostDTO post, int userId);
        bool DeleteBlogPost(int postId, int userId);
        IEnumerable<BlogPostDTO> GetAllBlogPosts();
        IEnumerable<BlogPostDTO> GetBlogPostsByUser(int userId);
        IEnumerable<CategoryDTO> GetAllCategories(); // Ny metod
        BlogPostDTO GetBlogPostById(int postId);

        IEnumerable<BlogPostDTO> SearchPostsByTitle(string title);
        IEnumerable<BlogPostDTO> SearchPostsByCategory(int categoryId);


    }
}
