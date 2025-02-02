using BlogCommunityAPI_Assignment2.DTO;
using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogCommunityAPI_Assignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;


        public LoginController(IAuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("login")]
        public IActionResult Login([FromQuery] LoginDTO loginDTO)
        {
            // Authenticate the user and generate a token
            var token = _authService.Authenticate(loginDTO.Username, loginDTO.PasswordHash);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid username or password."); // Return 401 if authentication fails
            }

            return Ok(new { Token = token, Message = "Login successful." }); // Return token on success
        }

    }
}
