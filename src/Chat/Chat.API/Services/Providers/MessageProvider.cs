﻿using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Helpers;
using Chat.API.DTOs.Requests;
using Chat.API.DTOs.Responses;
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

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="sendMessageRequest">Сообщение</param>
        /// <returns></returns>
        /// <exception cref="ProcessingException">При отправке сообщения произошла ошибка</exception>
        public async Task SendMessage(User sender, SendMessageRequestDTO sendMessageRequest)
        {
            // Получние диалога
            var dialogue = await _dialogueRepository.Get(sender, sendMessageRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Определение получателя сообщения/второго участника диалога
            var receiver = await _userRepository.Get(DialogueHelper.GetSecondDialogueMemberId(dialogue, sender.Id));
            if (receiver == null)
                throw new ProcessingException("Receiver not found!");

            // Инициализация модели DialogueMessage для записи в БД и оповещения обоих участников диалога
            var message = new DialogueMessage()
            {
                SenderId = sender.Id,
                DialogueId = sendMessageRequest.DialogueId,
                Content = sendMessageRequest.Content,
                CreatedDate = DateTime.UtcNow,
            };

            // Запись сообщения в БД
            await _dialogueRepository.AddMessage(dialogue, message);

            // Подгрузка подключений пользователя и отправка сообщения получателю
            await _connectionRepository.LoadConnections(receiver);
            await _messagingService.SendMessage(receiver, message);

            // Подгрузка подключений пользователя и отправка сообщения отправителю
            await _connectionRepository.LoadConnections(sender);
            await _messagingService.SendMessage(sender, message);
        }

        public async Task<GetMessagesResponseDTO> GetMessages(User user, GetMessagesRequestDTO getMessagesRequest)
        {
            // Получение диалога
            var dialogue = await _dialogueRepository.Get(user, getMessagesRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение списка сообщений диалога
            var messages = await _messageRepository.Get(dialogue, getMessagesRequest.Count, getMessagesRequest.Offset);
            // if (messages == null || !messages.Any())
            //     throw new ProcessingException("No messages!");

            // Возврат результата выполнения
            return new GetMessagesResponseDTO()
            {
                DialogueId = dialogue.Id,
                Messages = ToDialogueMessageDTOs(messages)
            };
        }

        public async Task DeleteMessages(User sender, DeleteMessagesRequestDTO deleteMessagesRequest)
        {
            // Получение диалога
            var dialogue = await _dialogueRepository.Get(sender, deleteMessagesRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            // Получение второго участника диалога
            var receiver = await _userRepository.Get(DialogueHelper.GetSecondDialogueMemberId(dialogue, sender.Id));
            if (receiver == null)
                throw new ProcessingException("Receiver not found!");

            // Получение сообщений удаляемого диалога из БД
            var messagesToDelete = await _messageRepository.Get(dialogue.Id, deleteMessagesRequest.MessageIds);
            if (messagesToDelete == null || !messagesToDelete.Any())
                throw new ProcessingException("Message not found!");

            // Удаление сообщений из БД
            await _messageRepository.Delete(messagesToDelete);

            // Иницализация модели для оповещения участников беседы
            var deletedMessage = new DeletedMessagesDTO()
            {
                DialogueId = deleteMessagesRequest.DialogueId,
                InitiatorId = sender.Id,
                MessageIds = deleteMessagesRequest.MessageIds
            };

            // Подгрузка подключений пользователя и отправка уведомления о удалении первому участнику диалога
            await _connectionRepository.LoadConnections(sender);
            await _messagingService.SendDeletedMessage(sender, deletedMessage);

            // Подгрузка подключений пользователя и отправка уведомления о удалении второму участнику диалога
            await _connectionRepository.LoadConnections(receiver);
            await _messagingService.SendDeletedMessage(receiver, deletedMessage);
        }

        public async Task UpdateMessage(User sender, UpdateMessageRequestDTO updateMessageRequest)
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

        private static ICollection<DialogueMessageDTO>? ToDialogueMessageDTOs(ICollection<DialogueMessage> dialogueMessages)
        {
            if (dialogueMessages == null)
                return null;

            var messageDTOs = new List<DialogueMessageDTO>();

            foreach (var message in dialogueMessages)
            {
                messageDTOs.Add
                (
                    new DialogueMessageDTO()
                    {
                        MessageId = message.Id,
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
