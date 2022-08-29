using Chat.API.Entities;

namespace Chat.API.Services.MessageRepositories
{
    public interface IMessageRepository
    {
        Task Add(DialogueMessage message);

        Task<ICollection<DialogueMessage>> GetMessages(Dialogue dialogue, int count, int offset);
    }
}
