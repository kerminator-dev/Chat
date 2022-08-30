using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Helpers;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.ConnectionRepositories;
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
        private readonly IConnectionRepository _connectionRepository;

        public MessageProvider(IMessageRepository messageRepository, IMessagingService messagingService, IUserRepository userRepository, IDialogueRepository dialogueRepository, IConnectionRepository connectionRepository)
        {
            _messageRepository = messageRepository;
            _messagingService = messagingService;
            _userRepository = userRepository;
            _dialogueRepository = dialogueRepository;
            _connectionRepository = connectionRepository;
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

            // Подгрузка подключений пользователя и отправка сообщения участнику диалога
            await _connectionRepository.LoadConnections(receiver);
            await _messagingService.SendMessage(receiver, messageModel);

            // Подгрузка подключений пользователя и отправка сообщения отправителю
            await _connectionRepository.LoadConnections(sender);
            await _messagingService.SendMessage(sender, messageModel);
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

        public async Task DeleteMessage(User sender, DeleteMessageRequest deleteMessageRequest)
        {
            // Получение диалога
            var dialogue = await _dialogueRepository.Get(sender, deleteMessageRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение получателя/второго участника диалога
            var receiver = await _userRepository.Get(DialogueHelper.GetSecondDialogueMemberId(dialogue, sender.Id));
            if (receiver == null)
                throw new ProcessingException("Receiver not found!");

            // Получение сообщения из БД, которое нужно удалить
            var messageToDelete = await _messageRepository.Get(dialogue.Id, deleteMessageRequest.MessageId);
            if (messageToDelete == null)
                throw new ProcessingException("Message not found!");

            // Удаление сообщения из БД
            await _messageRepository.Delete(messageToDelete);

            // Иницализация модели для оповещения
            var deletedMessage = new DeletedMessageDTO()
            {
                DialogueId = deleteMessageRequest.DialogueId,
                InitiatorId = sender.Id,
                MessageId = deleteMessageRequest.MessageId
            };

            // Подгрузка подключений пользователя и отправка уведомления о удалении первому участнику диалога
            await _connectionRepository.LoadConnections(sender);
            await _messagingService.SendDeletedMessage(sender, deletedMessage);

            // Подгрузка подключений пользователя и отправка уведомления о удалении второму участнику диалога
            await _connectionRepository.LoadConnections(receiver);
            await _messagingService.SendDeletedMessage(receiver, deletedMessage);
        }

        public async Task UpdateMessage(User sender, UpdateMessageRequest updateMessageRequest)
        {
            // Получение сообщения
            var message = await _messageRepository.Get(updateMessageRequest.DialogueId, updateMessageRequest.MessageId);
            if (message == null)
                throw new ProcessingException("Message not found!");

            // Получение диалога
            var dialogue = await _dialogueRepository.Get(sender, updateMessageRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение получателя/второго участника диалога
            var receiver = await _userRepository.Get(DialogueHelper.GetSecondDialogueMemberId(dialogue, sender.Id));
            if (receiver == null)
                throw new ProcessingException("Receiver not found!");

            // Изменение контента сообщения
            message.Content = updateMessageRequest.Content;

            // Обновление сообщения в БД
            await _messageRepository.Update(message);

            var updatedMessageDTO = new UpdatedMessageDTO()
            {
                InitiatorId = sender.Id,
                NewMessage = message
            };

            // Подгрузка подключений пользователя и отправка уведомления о обновлении сообщения первому участнику диалога
            await _connectionRepository.LoadConnections(sender);
            await _messagingService.SendUpdatedMessage(sender, updatedMessageDTO);

            // Подгрузка подключений пользователя и отправка уведомления о обновлении сообщения второму участнику диалога
            await _connectionRepository.LoadConnections(receiver);
            await _messagingService.SendUpdatedMessage(receiver, updatedMessageDTO);
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
