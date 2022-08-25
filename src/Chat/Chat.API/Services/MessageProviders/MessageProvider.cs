using Chat.API.DbContexts;
using Chat.API.Models;
using Chat.API.Models.Requests;
using Chat.API.Services.ConnectionRepositories;
using Chat.API.Services.MessageRepositories;
using Chat.API.Services.MessagingServices;
using Chat.API.Services.UserRepositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.Messangers
{
    public class MessageProvider
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessagingService _messagingService;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;

        public MessageProvider(IMessageRepository messageRepository, IMessagingService messagingService, IUserRepository userRepository, ApplicationDbContext dbContext, IConnectionRepository connectionRepository)
        {
            _messageRepository = messageRepository;
            _messagingService = messagingService;
            _userRepository = userRepository;
            _dbContext = dbContext;
            _connectionRepository = connectionRepository;
        }

        public async Task SendMessage(User sender, User receiver, SendMessageRequest message)
        {
            var messageModel = new Message()
            {
                SenderId = sender.UserId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                CreatedDate = DateTime.UtcNow,
            };

           await _dbContext.Entry<User>(receiver)
                .Collection(u => u.Connections)
                .LoadAsync();

            // Запись в БД
            await _messageRepository.Add(messageModel);

            // Отправка
            await _messagingService.SendMessage(receiver, messageModel);
        }
    }
}
