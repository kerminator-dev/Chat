using Chat.API.Models;
using Chat.API.Models.Responses;
using Chat.API.Services.RefreshTokenRepositories;
using Chat.API.Services.TokenGenerators;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Chat.API.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthenticationConfiguration _authenticationConfiguration;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
                             RefreshTokenGenerator refreshTokenGenerator, 
                             IRefreshTokenRepository refreshTokenRepository, 
                             AuthenticationConfiguration authenticationConfiguration)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationConfiguration = authenticationConfiguration;
        }

        public async Task<AuthenticatedUserResponce> Authenticate(User user)
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
    }
}
