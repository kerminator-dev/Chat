namespace Chat.API.DTOs
{
    /// <summary>
    /// Диалог с последним сообщением
    /// </summary>
    public class DialogueWithLastMessageDTO
    {
        /// <summary>
        /// Id диалога
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время создания диалога
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Id пользователя-создателя диалога
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Id второго участника диалога
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Последнее сообщение из диалога
        /// </summary>
        public DialogueMessageDTO? LastMessage { get; set; }
    }
}
