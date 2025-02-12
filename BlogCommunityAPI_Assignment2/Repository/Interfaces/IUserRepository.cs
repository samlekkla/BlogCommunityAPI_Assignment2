using BlogCommunityAPI_Assignment2.DTO;

namespace BlogCommunityAPI_Assignment2.Repository.Interfaces
{
    public interface IUserRepository
    {
        int CreateUser(string username, string passwordHash, string email); // Creates a new user and returns the UserId  
        User GetUserByUsername(string username); // Retrieves a user by their userId
        int UpdateUser(int userId, UpdateUserDTO userDTO);
        int DeleteUser(int userId);

    }
}
