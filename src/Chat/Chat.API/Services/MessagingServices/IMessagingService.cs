using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Services.MessagingServices
{
    public interface IMessagingService
    {
        Task SendMessage(User receiver, DialogueMessage newMessage);

        Task SendDeletedMessage(User receiver, DeletedMessagesDTO deletedMessage);

        Task SendUpdatedMessage(User receiver, UpdatedMessageDTO updatedMessage);

        Task SendCreatedDialogue(User receiver, CreatedDialogueDTO newDialogue);

        Task SendDeletedDialogue(User receiver, DeletedDialogueDTO deletedDialogue);
    }
}