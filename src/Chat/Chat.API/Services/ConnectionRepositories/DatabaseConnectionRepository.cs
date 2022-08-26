using Chat.API.DbContexts;
using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.ConnectionRepositories
{
    public class DatabaseConnectionRepository : IConnectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseConnectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Connection>> GetUserConnections(int userId)
        {
            return await _dbContext.Connections.Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
