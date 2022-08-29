using Chat.API.Entities;

namespace Chat.API.Services.MessagingServices
{
    public interface IMessagingService
    {
        Task SendMessage(User receiver, DialogueMessage message);
    }
}
