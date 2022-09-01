namespace Chat.API.DTOs
{
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
