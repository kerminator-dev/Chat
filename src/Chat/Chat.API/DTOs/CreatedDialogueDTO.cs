namespace Chat.API.DTOs
{
    /// <summary>
    /// Созданный диалог
    /// Нужен для оповещения пользователей о создании диалога
    /// </summary>
    public class CreatedDialogueDTO
    {
        /// <summary>
        /// ID диалога
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время создания диалога
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// ID User'а, создавшего диалог
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// ID User'а, участника диалога
        /// </summary>
        public int MemberId { get; set; }
    }
}
