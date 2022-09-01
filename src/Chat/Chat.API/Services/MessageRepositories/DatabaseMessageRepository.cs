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

        public async Task Delete(DialogueMessage message)
        {
            _dbContext.Messages.Remove(message);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(ICollection<DialogueMessage> messageIds)
        {
            _dbContext.Messages.RemoveRange(messageIds);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<DialogueMessage>> Get(int dialogueId, ICollection<int> messageIds)
        {
            return await _dbContext.Messages
                        .Where(m => m.DialogueId == dialogueId && messageIds.Contains(m.Id))
                        .ToListAsync();
        }

        public async Task<DialogueMessage> Get(int dialogueId, int messageId)
        {
            return await _dbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId && m.DialogueId == dialogueId);
        }

        public async Task<ICollection<DialogueMessage>> Get(Dialogue dialogue, int count, int offset = 0)
        {
            return await _dbContext.Messages.Where(m => m.DialogueId == dialogue.Id).OrderByDescending(m => m.Id).Skip(offset).Take(count).ToListAsync();
           // return await _dbContext.Messages.Where(m => m.DialogueId == dialogue.Id).TakeLast(count).ToListAsync();
        }

        public async Task Update(DialogueMessage message)
        {
            _dbContext.Messages.Update(message);

            await _dbContext.SaveChangesAsync();
        }
    }
}
