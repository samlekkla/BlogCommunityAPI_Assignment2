using Microsoft.IdentityModel.Logging;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface IAuthService
    {
        string Authenticate(string username, string password); // Returns JWT token
    }
}
