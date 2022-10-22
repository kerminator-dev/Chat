using Chat.API.Entities;
using Chat.API.Models;
using Chat.API.DTOs.Requests;
using Chat.API.DTOs.Responses;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.RefreshTokenRepositories;
using Chat.API.Services.TokenGenerators;
using Chat.API.Services.TokenValidators;
using Chat.API.Services.UserRepositories;
using System.Security.Claims;
using ErrorOr;

namespace Chat.API.Services.Providers
{
    public class AuthenticationProvider
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public AuthenticationProvider(AccessTokenGenerator accessTokenGenerator,
                                 RefreshTokenGenerator refreshTokenGenerator,
                                 IRefreshTokenRepository refreshTokenRepository,
                                 AuthenticationConfiguration authenticationConfiguration,
                                 IPasswordHasher passwordHasher,
                                 IUserRepository userRepository,
                                 RefreshTokenValidator refreshTokenValidator)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationConfiguration = authenticationConfiguration;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _refreshTokenValidator = refreshTokenValidator;
        }

        /// <summary>
        /// Выполнить аутентификацию
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<AuthenticatedUserResponseDTO> AuthenticateUser(User user)
        {
            // Генерация access-токена для пользователя
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            // Инициализация модели
            var refreshTokenModel = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(_authenticationConfiguration.RefreshTokenExpirationMinutes),
            };

            // Добавление в БД
            await _refreshTokenRepository.Create
            (
                refreshToken: refreshTokenModel
            );

            // Возврат результата
            return new AuthenticatedUserResponseDTO
            (
                user: new UserDTO()
                { 
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Color = user.Color,
                },
                accessToken: accessToken,
                accessTokenExpirationMinutes: _authenticationConfiguration.AccessTokenExpirationMinutes,
                refreshToken: refreshToken,
                refreshTokenExpirationMinutes: _authenticationConfiguration.RefreshTokenExpirationMinutes
            );
        }

        /// <summary>
        /// Получить пользователя из HttpContext'а 
        /// </summary>
        /// <param name="httpContextUser"></param>
        /// <returns></returns>
        public async Task<User> GetHttpContextUser(ClaimsPrincipal httpContextUser)
        {
            if (!int.TryParse(httpContextUser.FindFirstValue("id"), out int userId))
            {
                return null;
            }

            return await GetUser(userId);
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUser(int userId)
        {
            return await _userRepository.Get(userId);
        }

        /// <summary>
        /// Проучмит пользователя по Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> GetUser(string username)
        {
            return await _userRepository.Get(username);
        }

        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        public async Task RegisterUser(RegisterRequestDTO registerRequest)
        {
            // Генерация хэша пароля
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

            // Создание модели
            User user = new User()
            {
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
                Name = registerRequest.Name,
                Color = registerRequest.Color,
            };

            // Добавление в БД
            await _userRepository.Create(user);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            return _refreshTokenValidator.Validate(refreshToken);
        }
    }
}
