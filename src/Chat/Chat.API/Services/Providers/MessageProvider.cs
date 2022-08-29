using Chat.API.DbContexts;
using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.ConnectionRepositories;
using Chat.API.Services.DialogueRepositories;
using Chat.API.Services.MessageRepositories;
using Chat.API.Services.MessagingServices;
using Chat.API.Services.UserRepositories;
using Microsoft.EntityFrameworkCore;

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
            var dialogues = await _dialogueRepository.GetAll(sender);
            if (dialogues == null || dialogues.Count == 0)
                throw new ProcessingException("Dialogues not found!");

            var dialogue = dialogues.FirstOrDefault(d => d.Id == message.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            var receiver = await _userRepository.Get(GetReceiverId(dialogue, sender));
            if (receiver == null)
                throw new ProcessingException("Unknown receiver!");

            var messageModel = new DialogueMessage()
            {
                SenderId = sender.Id,
                DialogueId = message.DialogueId,
                Content = message.Content,
                CreatedDate = DateTime.UtcNow,
            };

            // Запись в БД
            await _dialogueRepository.AddMessage(dialogue, messageModel);

            // Отправка
            await _messagingService.SendMessage(receiver, messageModel);
        }

        public async Task<GetMessagesResponse> GetMessages(User user, GetMessagesRequest getMessagesRequest)
        {
            var dialogue = await _dialogueRepository.Get(user, getMessagesRequest.DialogueId);
            if (dialogue == null)
                throw new ProcessingException("Dialogue not found!");

            var messages = await _messageRepository.GetMessages(dialogue, getMessagesRequest.Count, getMessagesRequest.Offset);
            if (messages == null || messages.Count == 0)
                throw new ProcessingException("No messages!");

            
            return new GetMessagesResponse()
            {
                DialogueId = dialogue.Id,
                Messages = ToDialogueMessageDTOs(messages)
            };
        }

        private static int GetReceiverId(Dialogue dialogue, User sender)
        {
            if (dialogue.CreatorId == sender.Id)
                return dialogue.MemberId;

            return dialogue.CreatorId;
        }

        private static ICollection<DialogueMessageDTO> ToDialogueMessageDTOs(ICollection<DialogueMessage> dialogueMessages)
        {
            var messageDTOs = new List<DialogueMessageDTO>();

            foreach (var message in dialogueMessages)
            {
                messageDTOs.Add(new DialogueMessageDTO()
                {
                    Id = message.Id,
                    Content = message.Content,
                    Created = message.CreatedDate,
                    SenderId = message.SenderId
                });
            }

            return messageDTOs;
        }
    }
}
