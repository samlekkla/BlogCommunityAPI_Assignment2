using BlogCommunityAPI_Assignment2.Data;
using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Repos;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Skapa anv�ndare
        [HttpPost("register")]
        public IActionResult Register(string username, string password, string email)
        {
            _userRepository.CreateUser(username, password, email);
            return Ok("User created successfully.");
        }

        // Logga in anv�ndare
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var userId = _userRepository.LoginUser(username, password);
            if (userId.HasValue)
            {
                // H�r sparar vi sessionen p� servern. I praktiken kan vi ocks� anv�nda cookies eller tokens.
                HttpContext.Session.SetInt32("UserId", userId.Value);
                return Ok(new { UserId = userId });
            }
            return Unauthorized("Invalid credentials.");
        }

        // H�mta anv�ndarinformation
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Unauthorized("User not logged in.");

            var user = _userRepository.GetUserById(userId.Value);
            return Ok(user);
        }

        // Uppdatera anv�ndarinformation
        [HttpPut("update")]
        public IActionResult UpdateProfile(string email)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Unauthorized("User not logged in.");

            _userRepository.UpdateUser(userId.Value, email);
            return Ok("User profile updated.");
        }

        // Ta bort anv�ndare
        [HttpDelete("delete")]
        public IActionResult DeleteUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Unauthorized("User not logged in.");

            _userRepository.DeleteUser(userId.Value);
            return Ok("User deleted.");
        }
    }
}
