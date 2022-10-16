namespace Chat.API.DTOs
{
    /// <summary>
    /// Удалённые сообщения
    /// Нужен для оповещения участников диалога о том, 
    /// что были удалены сообщения
    /// </summary>
    public class DeletedMessagesDTO
    {
        /// <summary>
        /// ID диалога
        /// </summary>
        public int DialogueId { get; set; }

        /// <summary>
        /// ID удалённого сообщения
        /// </summary>
        public List<int> MessageIds { get; set; }

        /// <summary>
        /// ID пользователя, удалившего сообщение
        /// </summary>
        public int InitiatorId { get; set; }
    }
}
