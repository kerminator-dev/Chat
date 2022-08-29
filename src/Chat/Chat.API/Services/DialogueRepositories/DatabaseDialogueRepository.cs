using Chat.API.DbContexts;
using Chat.API.DTOs;
using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.DialogueRepositories
{
    public class DatabaseDialogueRepository : IDialogueRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseDialogueRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Dialogue dialogue)
        {
            _dbContext.Dialogues.Add(dialogue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Dialogue>> GetAll(User user)
        {
            return await _dbContext.Dialogues
                .Where(d => d.CreatorId == user.Id || d.MemberId == user.Id)
                .ToListAsync();
        }

        public async Task Delete(Dialogue dialogue)
        {
            _dbContext.Remove(dialogue);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddMessage(Dialogue dialogue, DialogueMessage message)
        {
            await _dbContext.Messages.AddAsync(message);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dialogue> Get(User user, int dialogueId)
        {
            return await _dbContext
                        .Dialogues
                        .FirstOrDefaultAsync(d => d.Id == dialogueId && (d.MemberId == user.Id || d.CreatorId == user.Id));
        }

        public async Task<bool> Any(User user1, User user2)
        {
            return await _dbContext
                    .Dialogues
                    .AnyAsync(d => 
                        (d.CreatorId == user1.Id && d.MemberId == user2.Id) || 
                        (d.MemberId == user1.Id && d.CreatorId == user2.Id));
        }

        public async Task<ICollection<Dialogue>> GetDialoguesWithLastMessages(User user)
        {
            //return await _dbContext.Dialogues
            //             .Where(d => d.CreatorId == user.Id || d.MemberId == user.Id)
            //             .Include(d => d.Messages.OrderByDescending(m => m.Id).Take(1))
            //             .ToListAsync();
            return await _dbContext.Dialogues
                         .Where(d => d.CreatorId == user.Id || d.MemberId == user.Id)
                         .Include(d => d.Messages.TakeLast(1))
                         .ToListAsync();
        }
    }
}
