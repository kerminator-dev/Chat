using Chat.API.Entities;

namespace Chat.API.Services.ConversationRepositories
{
    public interface IConversationRepository
    {
        Task CreateConversation(Conversation conversation);

        public Task<ICollection<Conversation>> GetAllUserConversations(User user);
    }
}
