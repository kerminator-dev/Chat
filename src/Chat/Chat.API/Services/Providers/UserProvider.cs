using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.UserRepositories;

namespace Chat.API.Services.Providers
{
    public class UserProvider
    {
        private readonly IUserRepository _userRepository;

        public UserProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SearchUserResponse> Search(SearchUserRequest searchUserRequest)
        {
            var foundUsers = await _userRepository.Search(searchUserRequest.Username);
            if (foundUsers == null || !foundUsers.Any())
            {
                throw new ProcessingException("Users not found!");
            }

            return new SearchUserResponse()
            {
                Users = ToUserDTOs(foundUsers)
            };
        }

        private ICollection<UserDTO> ToUserDTOs(ICollection<User> users)
        {
            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = new UserDTO()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username
                };

                result.Add(userDTO);
            }

            return result;
        }
    }
}
