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

        public async Task<ICollection<HubConnection>> GetUserConnections(int userId)
        {
            return await _dbContext.Connections.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<User> LoadConnections(User user)
        {
            if (user == null)
                return user;

            await _dbContext.Entry(user).Collection(u => u.Connections).LoadAsync();

            return user;
        }
    }
}
