using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Helpers;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.DialogueRepositories;
using Chat.API.Services.MessageRepositories;
using Chat.API.Services.MessagingServices;
using Chat.API.Services.UserRepositories;

namespace Chat.API.Services.Providers
{
    // TODO - Обработка и возвращение ошибок

    public class MessageProvider
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessagingService _messagingService;
        private readonly IUserRepository _userRepository;
        private readonly IDialogueRepository _dialogueRepository;

        public MessageProvider(IMessageRepository messageRepository, IMessagingService messagingService, IUserRepository userRepository, IDialogueRepository dialogueRepository)
        {
            _messageRepository = messageRepository;
            _messagingService = messagingService;
            _userRepository = userRepository;
            _dialogueRepository = dialogueRepository;
        }

        public async Task SendMessage(User sender, SendMessageRequest message)
        {
            // Получние диалога
            var dialogue = await _dialogueRepository.Get(sender, message.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение получателя/второго участника диалога
            var receiver = await _userRepository.Get(DialogueHelper.GetSecondDialogueMemberId(dialogue, sender.Id));
            if (receiver == null)
                throw new ProcessingException("Receiver not found!");

            // Инициализация модели DialogueMessage
            var messageModel = new DialogueMessage()
            {
                SenderId = sender.Id,
                DialogueId = message.DialogueId,
                Content = message.Content,
                CreatedDate = DateTime.UtcNow,
            };

            // Запись в БД
            await _dialogueRepository.AddMessage(dialogue, messageModel);

            // Отправка сообщения участнику диалога
            await _messagingService.SendMessage(receiver, messageModel);

            // Отправка сообщения отправителю
            await _messagingService.SendMessage(receiver, messageModel);
        }

        public async Task<GetMessagesResponse> GetMessages(User user, GetMessagesRequest getMessagesRequest)
        {
            // Получение диалога
            var dialogue = await _dialogueRepository.Get(user, getMessagesRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение списка сообщений диалога
            var messages = await _messageRepository.GetMessages(dialogue, getMessagesRequest.Count, getMessagesRequest.Offset);
            if (messages == null || messages.Count == 0)
                throw new ProcessingException("No messages!");

            // Возврат результата выполнения
            return new GetMessagesResponse()
            {
                DialogueId = dialogue.Id,
                Messages = ToDialogueMessageDTOs(messages)
            };
        }

        private static ICollection<DialogueMessageDTO> ToDialogueMessageDTOs(ICollection<DialogueMessage> dialogueMessages)
        {
            var messageDTOs = new List<DialogueMessageDTO>();

            foreach (var message in dialogueMessages)
            {
                messageDTOs.Add
                (
                    new DialogueMessageDTO()
                    {
                        Id = message.Id,
                        Content = message.Content,
                        Created = message.CreatedDate,
                        SenderId = message.SenderId
                    }
                );
            }

            return messageDTOs;
        }
    }
}
