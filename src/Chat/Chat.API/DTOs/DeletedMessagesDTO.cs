namespace Chat.API.DTOs
{
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
