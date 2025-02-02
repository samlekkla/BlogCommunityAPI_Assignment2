using AutoMapper;
using BlogCommunityAPI_Assignment2.DTO;

namespace BlogCommunityAPI_Assignment2.MappingProfile
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // User mapping
            CreateMap<User, UserDTO>(); // Maps User -> UserDTO
            CreateMap<CreateUserDTO, User>(); // Maps CreateUserDTO -> User

        }   
    }
}
