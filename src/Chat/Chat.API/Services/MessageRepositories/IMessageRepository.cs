using Chat.API.Entities;

namespace Chat.API.Services.MessageRepositories
{
    public interface IMessageRepository
    {
        Task Add(Message message);
    }
}
