using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Services.NotificationServices
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRNotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyConnectionStatus(User receiver, ConnectionStatusDTO connectionStatus)
        {
            await this.Send<ConnectionStatusDTO>(receiver, "UserConnectionStatus", connectionStatus);
        }

        /// <summary>
        /// Отправить объект на все действительные подключения пользователя
        /// </summary>
        /// <typeparam name="T">Тип отправляемого объект</typeparam>
        /// <param name="receiver">Получатель</param>
        /// <param name="methodName">Название метода-события на стороне получателя</param>
        /// <param name="objectToSend">Отправляемый объект</param>
        /// <returns></returns>
        protected async Task Send<T>(User receiver, string methodName, T objectToSend) where T : class
        {
            if (receiver.Connections is null)
                return;

            // Отправка объекта на все действительные подключения пользователя
            foreach (var connection in receiver.Connections.Where(c => c.Connected))
            {
                await _hubContext.Clients.Client(connection.Id).SendAsync(methodName, objectToSend);
            }
        }
    }
}
