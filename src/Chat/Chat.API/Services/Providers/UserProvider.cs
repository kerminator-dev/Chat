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
            // Поиск пользователей
            var foundUsers = await _userRepository.Search(searchUserRequest.Username);
            if (foundUsers == null || !foundUsers.Any())
            {
                throw new ProcessingException("Users not found!");
            }

            // Возврат результата
            return new SearchUserResponse()
            {
                Users = ToUserDTOs(foundUsers)
            };
        }

        public async Task<GetUsersResponse> Get(GetUsersRequest getUsersRequest)
        {
            // Удаление дупликатов
            var userIds = getUsersRequest.UserIds.Distinct().ToList();
            if (userIds == null || !userIds.Any())
                throw new ProcessingException("There is no users in request!");

            var users = await _userRepository.Get(userIds);
            if (users == null || !users.Any())
                throw new ProcessingException("Users not found!");

            return new GetUsersResponse()
            {
                Users = ToUserDTOs(users)
            };
        }

        private static ICollection<UserDTO> ToUserDTOs(ICollection<User> users)
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
