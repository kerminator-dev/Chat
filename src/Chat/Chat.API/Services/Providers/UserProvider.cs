using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.DTOs.Requests;
using Chat.API.DTOs.Responses;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.UserRepositories;

namespace Chat.API.Services.Providers
{
    public class UserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserProvider(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Найти пользователей по никнейму
        /// </summary>
        /// <param name="searchUserRequest">Запрос</param>
        /// <returns></returns>
        /// <exception cref="ProcessingException"></exception>
        public async Task<SearchUserResponseDTO> SearchUsers(SearchUserRequestDTO searchUserRequest)
        {
            // Поиск пользователей
            var foundUsers = await _userRepository.Search(searchUserRequest.Username);
            if (foundUsers == null || !foundUsers.Any())
            {
                throw new ProcessingException("Users not found!");
            }

            // Возврат результата
            return new SearchUserResponseDTO()
            {
                Users = ToUserDTOs(foundUsers)
            };
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="getUsersRequest"></param>
        /// <returns></returns>
        /// <exception cref="ProcessingException"></exception>
        public async Task<GetUsersResponseDTO> GetUsers(GetUsersRequestDTO getUsersRequest)
        {
            // Удаление дубликатов
            var userIds = getUsersRequest.UserIds.Distinct().ToList();
            if (userIds == null || !userIds.Any())
                throw new ProcessingException("There is no users in request!");

            // Получение списка пользователей
            var users = await _userRepository.Get(userIds);
            if (users == null || !users.Any())
                throw new ProcessingException("Users not found!");

            // Преобразование в DTO и возврат
            return new GetUsersResponseDTO()
            {
                Users = ToUserDTOs(users)
            };
        }

        /// <summary>
        /// Обновить пароль для пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="updatePasswordRequest">Запрос на изменение пароля</param>
        /// <returns></returns>
        /// <exception cref="ProcessingException"></exception>
        public async Task UpdatePassword(User user, UpdatePasswordRequestDTO updatePasswordRequest)
        {
            if (updatePasswordRequest.OldPassword == updatePasswordRequest.NewPassword)
                throw new ProcessingException("New and old passwords are the same!");

            var passwordIsCorrect = _passwordHasher.VerifyPassword(updatePasswordRequest.OldPassword, user.PasswordHash);
            // Если старые пароли не совпадают, то прервать изменение пароля
            if (!passwordIsCorrect)
                throw new ProcessingException("Old password is not correct!");

            // Хэширование нового пароля
            var newPasswordHash = _passwordHasher.HashPassword(updatePasswordRequest.NewPassword);

            // Запись хэша в Entity
            user.PasswordHash = newPasswordHash;

            // Сохранение Entity в БД
            await _userRepository.Update(user);
        }

        public async Task UpdateColor(User user, int color)
        {
            user.Color = color;

            await _userRepository.Update(user);
        }

        public async Task UpdateName(User user, string newName)
        {
            user.Name = newName;

            await _userRepository.Update(user);
        }

        /// <summary>
        /// Преобразовать коллекцию типа User в коллекцию UserDTO 
        /// </summary>
        /// <param name="users">Коллекция пользователей</param>
        /// <returns>Коллекия пользователей DTO</returns>
        private static ICollection<UserDTO> ToUserDTOs(ICollection<User> users)
        {
            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = new UserDTO()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    Color = user.Color,
                };

                result.Add(userDTO);
            }

            return result;
        }
    }
}
