using Chat.API.Models;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.RefreshTokenRepositories;
using Chat.API.Services.TokenGenerators;
using Chat.API.Services.TokenValidators;
using Chat.API.Services.UserRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Chat.API.Services.Authenticators
{
    public class AuthenticationProvider
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AuthenticationConfiguration _authenticationConfiguration;
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

        public async Task<AuthenticatedUserResponce> AuthenticateUser(User user)
        {
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            var refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.UserId,
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(_authenticationConfiguration.RefreshTokenExpirationMinutes),
            };

            await _refreshTokenRepository.Create
            (
                refreshToken: refreshTokenDTO
            );

            return new AuthenticatedUserResponce
            (
                accessToken: accessToken,
                accessTokenExpirationMinutes: _authenticationConfiguration.AccessTokenExpirationMinutes,
                refreshToken: refreshToken,
                refreshTokenExpirationMinutes: _authenticationConfiguration.RefreshTokenExpirationMinutes
            );
        }

        public async Task<User> GetUser(string userId)
        {
            return await _userRepository.GetByUserId(userId);
        }

        public async Task RegisterUser(RegisterRequest registerRequest)
        {
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                UserId = registerRequest.UserId,
                PasswordHash = passwordHash,
                GivenName = registerRequest.GivenName,
            };

            await _userRepository.Create(registrationUser);
        }
    }
}
