using Chat.API.DbContexts;
using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.MessageRepositories
{
    public class DatabaseMessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseMessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(DialogueMessage message)
        {
            await _dbContext.Messages.AddAsync(message);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<DialogueMessage>> GetMessages(Dialogue dialogue, int count, int offset = 0)
        {
            return await _dbContext.Messages.Where(m => m.DialogueId == dialogue.Id).OrderByDescending(m => m.Id).Skip(offset).Take(count).ToListAsync();
           // return await _dbContext.Messages.Where(m => m.DialogueId == dialogue.Id).TakeLast(count).ToListAsync();
        }
    }
}
