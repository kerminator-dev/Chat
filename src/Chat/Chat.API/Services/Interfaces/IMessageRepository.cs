using Chat.API.Entities;

namespace Chat.API.Services.Interfaces
{
    public interface IMessageRepository
    {
        Task Add(DialogueMessage message);

        Task Delete(DialogueMessage message);

        Task Delete(ICollection<DialogueMessage> messageIds);

        Task<DialogueMessage> Get(int dialogueId, int messageId);

        Task<ICollection<DialogueMessage>> Get(int dialogueId, ICollection<int> MessageIDs);

        Task<ICollection<DialogueMessage>> Get(int dialogueId, int count, int offset);

        Task Update(DialogueMessage message);
    }
}
