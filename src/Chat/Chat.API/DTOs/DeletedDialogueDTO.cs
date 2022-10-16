namespace Chat.API.DTOs
{
    /// <summary>
    /// Удалённый диалог
    /// Нужен для оповещения участников диалога о удалении диалога 
    /// </summary>
    public class DeletedDialogueDTO
    {
        /// <summary>
        /// ID диалога
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID пользователя, удалившего диалог
        /// </summary>
        public int InitiatorId { get; set; }
    }
}
