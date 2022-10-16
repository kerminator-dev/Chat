using Chat.API.Entities;

namespace Chat.API.DTOs
{
    /// <summary>
    /// Изменённое сообщение
    /// Для уведомления участников диалога о том, 
    /// что сообщение было изменено
    /// </summary>
    public class UpdatedMessageDTO
    {
        /// <summary>
        /// Id пользователя, который изменил сообщение
        /// </summary>
        public int InitiatorId { get; set; }

        /// <summary>
        /// Изменённое сообщение
        /// </summary>
        public DialogueMessage NewMessage { get; set; }
    }
}
