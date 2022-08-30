using Chat.API.Hubs;
using Chat.API.Entities;
using Microsoft.AspNetCore.SignalR;
using Chat.API.DTOs;

namespace Chat.API.Services.MessagingServices
{
    public class SignalRMessagingService : IMessagingService
    {
        protected readonly IHubContext<ChatHub> _hubContext;

        public SignalRMessagingService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Уведомление пользователя о новом сообщении
        /// </summary>
        /// <param name="receiver">Получатель</param>
        /// <param name="newMessage">Новое сообщение</param>
        /// <returns></returns>
        public async Task SendMessage(User receiver, DialogueMessage newMessage)
        {
            await this.Send<DialogueMessage>(receiver, "ReceiveMessage", newMessage);
        }

        /// <summary>
        /// Уведомление пользователя о удалении сообщения
        /// </summary>
        /// <param name="receiver">Получатель</param>
        /// <param name="deletedMessage">Удалённое сообщение</param>
        /// <returns></returns>
        public async Task SendDeletedMessage(User receiver, DeletedMessageDTO deletedMessage)
        {
            await this.Send<DeletedMessageDTO>(receiver, "DeleteMessage", deletedMessage);
        }

        /// <summary>
        /// Уведомление пользователя о изменении сообщения
        /// </summary>
        /// <param name="receiver">Получатель</param>
        /// <param name="updatedMessage">Изменённое сообщение</param>
        /// <returns></returns>
        public async Task SendUpdatedMessage(User receiver, UpdatedMessageDTO updatedMessage)
        {
            await this.Send<UpdatedMessageDTO>(receiver, "UpdateMessage", updatedMessage);
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
