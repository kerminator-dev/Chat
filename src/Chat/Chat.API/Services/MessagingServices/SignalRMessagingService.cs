using Chat.API.Hubs;
using Chat.API.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.MessagingServices
{
    public class SignalRMessagingService : IMessagingService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRMessagingService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessage(User receiver, DialogueMessage message)
        {
            if (receiver.Connections == null)
                return;

            foreach (var connection in receiver.Connections)
            {
               await _hubContext.Clients.Client(connection.Id).SendAsync("ReceiveMessage", message);
            }
        }
    }
}
