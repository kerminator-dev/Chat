using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Services.MessagingServices
{
    public interface IMessagingService
    {
        Task SendMessage(User receiver, DialogueMessage newMessage);

        Task SendDeletedMessage(User receiver, DeletedMessageDTO deletedMessage);

        Task SendUpdatedMessage(User receiver, UpdatedMessageDTO updatedMessage);
    }
}