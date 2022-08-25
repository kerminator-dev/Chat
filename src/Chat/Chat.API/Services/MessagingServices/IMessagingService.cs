using Chat.API.Models;
using Chat.API.Models.Requests;

namespace Chat.API.Services.MessagingServices
{
    public interface IMessagingService
    {
        Task SendMessage(User receiver, Message message);
    }
}
