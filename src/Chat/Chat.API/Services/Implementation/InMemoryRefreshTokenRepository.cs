using Chat.API.Entities;
using Chat.API.Services.Interfaces;

namespace Chat.API.Services.Implementation
{
    /// <summary>
    /// Для теста без БД
    /// </summary>
    public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        // Dictionary<User<List<RefreshTokem>>
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public Task ClearCache()
        {
            return Task.CompletedTask;
        }

        public Task Create(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            _refreshTokens.RemoveAll(t => t.Id == id);

            return Task.CompletedTask;
        }

        public Task DeleteAllUserTokens(int userId)
        {
            _refreshTokens.RemoveAll(t => t.UserId == userId);

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(t => t.Token == token);

            return Task.FromResult(refreshToken);
        }
    }
}
