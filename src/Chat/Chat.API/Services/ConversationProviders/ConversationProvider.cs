using Chat.API.Entities;
using Chat.API.Models.Requests;
using Chat.API.Services.ConversationRepositories;

namespace Chat.API.Services.ConversationProviders
{
    public class ConversationProvider
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationProvider(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<ICollection<Conversation>> GetAllUserConversations(User requestor)
        {
            return await _conversationRepository.GetAllUserConversations(requestor);
        }

        public async Task CreateConversation(CreateConversationRequest createConversationRequest, User requestor)
        {

        }
    }
}
