using BlogCommunityAPI_Assignment2.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogCommunityAPI_Assignment2.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository; // Dependency for accessing user data
        private readonly string _jwtSecret; // Store the JWT secret key


        // Constructor that takes IUserRepository and the JWT secret key
        public AuthService(IUserRepository userRepository, string jwtSecret)
        {
            // Ensure userRepository is not null; throw an exception if it is

            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            // Ensure jwtSecret is not null; throw an exception if it is

            _jwtSecret = jwtSecret ?? throw new ArgumentNullException(nameof(jwtSecret));
        }
        // Autentisera med användar-ID och lösenord


        // Method to authenticate a user and generate a JWT token
        public string Authenticate(string username, string password)
        {
            // Validate user credentials
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || user.PasswordHash != password)
            {
                return null; // Invalid credentials
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // Viktigt för att kunna hämta UserID i controller
            new Claim("UserID", user.UserID.ToString()), // Extra säkerhet
            new Claim(ClaimTypes.Role, "User") // Assign role claim
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "http://localhost:5062",
                Audience = "http://localhost:5062",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); // Skapa token här
            return tokenHandler.WriteToken(token); // Nu används token korrekt
        }
    }
}

