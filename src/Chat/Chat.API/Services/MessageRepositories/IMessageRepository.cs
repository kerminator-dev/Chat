using Chat.API.Models;

namespace Chat.API.Services.MessageRepositories
{
    public interface IMessageRepository
    {
        Task Add(Message message);
    }
}
