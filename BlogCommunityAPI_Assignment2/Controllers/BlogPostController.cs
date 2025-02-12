using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using BlogCommunityAPI_Assignment2.Repository.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommentRepository _commentRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAllBlogPosts()
        {
            var posts = _blogPostRepository.GetAllBlogPosts();
            return Ok(posts);
        }

        [HttpGet("categories")]
        public IActionResult GetAllCategories()
        {
            var categories = _blogPostRepository.GetAllCategories();
            return Ok(categories);
        }

        [Authorize]
        [HttpPost("CreateBlogPost")]
        public IActionResult CreateBlogPost([FromBody] BlogPostDTO postDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value); // Hämtar inloggad användares ID
            var postId = _blogPostRepository.CreateBlogPost(postDTO, userId); // Skickar userId och CategoryID

            return CreatedAtAction(nameof(GetAllBlogPosts), new { id = postId }, postDTO);
        }

        [Authorize]
        [HttpPut("update/{postId}")]
        public IActionResult UpdateBlogPost(int postId, [FromBody] BlogPostDTO postDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value); // Hämtar den inloggade användarens ID

            // Vi skickar inte med userId eftersom det redan är hämtat ovan
            bool updated = _blogPostRepository.UpdateBlogPost(postId, postDTO, userId);

            if (updated)
            {
                return Ok(new { message = "Blogginlägget har uppdaterats." });
            }
            else
            {
                return Forbid(); // Förbjuder uppdatering om användaren inte är ägaren
            }
        }

        [Authorize]
        [HttpDelete("{postId}")]
        public IActionResult DeleteBlogPost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);

            bool deleted = _blogPostRepository.DeleteBlogPost(postId, userId); // Använd bool istället för int

            if (!deleted)
                return NotFound("Post not found or unauthorized.");

            return Ok("Post deleted successfully.");
        }



        [HttpGet("search/title")]
        public IActionResult SearchByTitle([FromQuery] string title)
        {
            var results = _blogPostRepository.SearchPostsByTitle(title);

            if (!results.Any())
            {
                return NotFound("Inga inlägg hittades.");
            }

            return Ok(results);
        }

        [HttpGet("search/category")]
        public IActionResult SearchByCategory([FromQuery] int categoryId)
        {
            var results = _blogPostRepository.SearchPostsByCategory(categoryId);

            if (!results.Any())
            {
                return NotFound("Inga inlägg hittades i denna kategori.");
            }

            return Ok(results);
        }



    }
}