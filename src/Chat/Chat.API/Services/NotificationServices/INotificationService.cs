using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Services.NotificationServices
{
    public interface INotificationService
    {
        /// <summary>
        /// Уведомить пользователя о статусе подключения другого пользователя (connectionStatus.UserId)
        /// </summary>
        /// <param name="receiver">Получатель уведомления</param>
        /// <param name="connectionStatus">Статус подключения</param>
        /// <returns></returns>
        Task NotifyConnectionStatus(User receiver, ConnectionStatusDTO connectionStatus);
    }
}
