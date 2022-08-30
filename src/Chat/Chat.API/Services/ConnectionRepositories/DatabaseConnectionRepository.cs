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

        public async Task Add(User user, HubConnection connection)
        {
            // Подгрузка подключений пользователя из БД, если их нет
            if (user.Connections == null)
                await this.LoadConnections(user);

            user.Connections?.Add(connection);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<HubConnection?> Get(string connectionId)
        {
            return await _dbContext.Connections.FindAsync(connectionId);
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

        public async Task SetConnectionStatus(HubConnection hubConnection, bool isActiveStatus)
        {
            _dbContext.Entry(hubConnection)
                .Entity.Connected = isActiveStatus;

            await _dbContext.SaveChangesAsync();
        }
    }
}
