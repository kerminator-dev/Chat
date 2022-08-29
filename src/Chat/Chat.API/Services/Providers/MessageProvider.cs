using Chat.API.DbContexts;
using Chat.API.Entities;
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
    public class MessageProvider
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessagingService _messagingService;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDialogueRepository _dialogueRepository;

        public MessageProvider(IMessageRepository messageRepository, IMessagingService messagingService, IUserRepository userRepository, ApplicationDbContext dbContext, IConnectionRepository connectionRepository, IDialogueRepository dialogueRepository)
        {
            _messageRepository = messageRepository;
            _messagingService = messagingService;
            _userRepository = userRepository;
            _dbContext = dbContext;
            _connectionRepository = connectionRepository;
            _dialogueRepository = dialogueRepository;
        }

        public async Task SendMessage(User sender, SendMessageRequest message)
        {
            var dialogues = await _dialogueRepository.GetAll(sender);

            if (dialogues == null || dialogues.Count == 0)
                return;

            var dialogue = dialogues.FirstOrDefault(d => d.Id == message.DialogueId);
            if (dialogue == null)
                return;

           //
           // var receiver = dialogue.Members.FirstOrDefault(m => m.Id != sender.Id);
           // if (receiver == null)
           //     return;

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
            // await _messagingService.SendMessage(receiver, messageModel);
        }

        public async Task<GetMessagesResponse> GetMessages(User user, GetMessagesRequest getMessagesRequest)
        {
            var dialogue = await _dialogueRepository.Get(user, getMessagesRequest.DialogueId);
            if (dialogue == null)
                return null;

            var messages = await _messageRepository.GetMessages(dialogue, getMessagesRequest.Count, getMessagesRequest.Offset);

            return new GetMessagesResponse()
            {
                Messages = messages
            };
        }
    }
}
