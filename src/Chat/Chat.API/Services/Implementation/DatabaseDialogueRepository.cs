using Chat.API.DbContexts;
using Chat.API.Entities;
using Chat.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.Implementation
{
    public class DatabaseDialogueRepository : IDialogueRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseDialogueRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dialogue> Create(Dialogue dialogue)
        {
            var result = await _dbContext.Dialogues.AddAsync(dialogue);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ICollection<Dialogue>> GetAll(int userId)
        {
            return await _dbContext.Dialogues
                .Where(d => d.CreatorId == userId || d.MemberId == userId)
                .ToListAsync();
        }

        public async Task Delete(Dialogue dialogue)
        {
            _dbContext.Remove(dialogue);

            await _dbContext.SaveChangesAsync();
        }

        //public async Task AddMessage(Dialogue dialogue, DialogueMessage message)
        //{
        //    await _dbContext.Messages.AddAsync(message);

        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task<Dialogue> Get(int userId, int dialogueId)
        {
            return await _dbContext
                        .Dialogues
                        .FirstOrDefaultAsync(d => d.Id == dialogueId && (d.MemberId == userId || d.CreatorId == userId));
        }

        public async Task<bool> Any(int userId1, int userId2)
        {
            return await _dbContext
                    .Dialogues
                    .AnyAsync(d =>
                        d.CreatorId == userId1 && d.MemberId == userId2 ||
                        d.MemberId == userId1 && d.CreatorId == userId2);
        }

        public async Task<ICollection<Dialogue>> GetDialoguesWithLastMessages(int userId)
        {
            return await _dbContext.Dialogues
                         .Where(d => d.CreatorId == userId || d.MemberId == userId)
                         .Include(d => d.Messages.OrderByDescending(m => m.Id).Take(1))
                         .ToListAsync();
        }
    }
}
