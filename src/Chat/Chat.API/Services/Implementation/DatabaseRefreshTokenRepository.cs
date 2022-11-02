using Chat.API.DbContexts;
using Chat.API.Entities;
using Chat.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.Implementation
{
    public class DatabaseRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseRefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ClearCache()
        {
            var tokensToDelete = await _dbContext.RefreshTokens.Where(t => t.ExpirationDateTime < DateTime.UtcNow).ToListAsync();

            if (tokensToDelete != null)
            {
                _dbContext.RemoveRange(tokensToDelete);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Create(RefreshToken refreshToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            RefreshToken refreshToken = await _dbContext.RefreshTokens.FindAsync(id);

            if (refreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(refreshToken);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}
