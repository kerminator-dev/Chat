using Chat.API.DbContexts;
using Chat.API.Entities;
using Chat.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.Implementation
{
    public class DatabaseConnectionRepository : IConnectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseConnectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(HubConnection connection)
        {

            await _dbContext.Connections.AddAsync(connection);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<HubConnection?> Get(string connectionId)
        {
            return await _dbContext.Connections.FindAsync(connectionId);
        }

        public async Task<ICollection<HubConnection>> Get(int userId)
        {
            return await _dbContext.Connections.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task SetConnectionStatus(HubConnection hubConnection, bool isActiveStatus)
        {
            _dbContext.Entry(hubConnection)
                .Entity.Connected = isActiveStatus;

            await _dbContext.SaveChangesAsync();
        }
    }
}
