using Chat.API.DbContexts;
using Chat.API.Entities;

namespace Chat.API.Services.ConversationRepositories
{
    public class DatabaseConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseConversationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Conversation>> GetAllUserConversations(User user)
        {
            await _dbContext.Entry(user)
                .Collection(u => u.Conversations).LoadAsync();

            return user.Conversations;
        }

        public async Task CreateConversation(Conversation conversation)
        {
            await _dbContext.Conversations.AddAsync(conversation);
        }
    }
}
