namespace Chat.API.Entities
{
    /// <summary>
    /// Сообщение из диалога
    /// </summary>
    public class DialogueMessage
    {
        /// <summary>
        /// Id сообщения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id диалога
        /// </summary>
        public int DialogueId { get; set; }

        /// <summary>
        /// Id отправителя сообщения
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Контент сообщения
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата и время создания/отправки
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
