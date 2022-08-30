namespace Chat.API.DTOs
{
    public class DeletedMessageDTO
    {
        /// <summary>
        /// ID диалога
        /// </summary>
        public int DialogueId { get; set; }

        /// <summary>
        /// ID удалённого сообщения
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// ID пользователя, удалившего сообщение
        /// </summary>
        public int InitiatorId { get; set; }
    }
}
