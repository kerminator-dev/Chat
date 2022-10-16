namespace Chat.API.DTOs
{
    /// <summary>
    /// Сообщение из диалога
    /// Для оповещения участников диалога о новом сообщении
    /// </summary>
    public class DialogueMessageDTO
    {
        /// <summary>
        /// Id сообщения
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Id отправителя
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Контент сообщения
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата и время отправки/создания
        /// </summary>
        public DateTime Created { get; set; }
    }
}
