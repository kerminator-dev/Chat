using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.MessageRepositories
{
    public class DatabaseMessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseMessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Message message)
        {
            await _dbContext.Messages.AddAsync(message);

            await _dbContext.SaveChangesAsync();
        }
    }
}
