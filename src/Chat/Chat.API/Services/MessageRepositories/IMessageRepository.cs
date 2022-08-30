using Chat.API.Entities;

namespace Chat.API.Services.MessageRepositories
{
    public interface IMessageRepository
    {
        Task Add(DialogueMessage message);

        Task Delete(DialogueMessage message);

        Task<DialogueMessage> Get(int dialogueId, int messageId);

        Task<ICollection<DialogueMessage>> GetMessages(Dialogue dialogue, int count, int offset);

        Task Update(DialogueMessage message);
    }
}
