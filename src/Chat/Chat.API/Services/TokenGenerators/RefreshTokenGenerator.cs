using Chat.API.Entities;
using Chat.API.Models;
using System.Security.Claims;

namespace Chat.API.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken
            (
                secretKey: _configuration.RefreshTokenSecret,
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                expirationMinutes: _configuration.RefreshTokenExpirationMinutes
            );
        }
    }
}
