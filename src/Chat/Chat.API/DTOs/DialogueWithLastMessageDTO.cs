namespace Chat.API.DTOs
{
    public class DialogueWithLastMessageDTO
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int CreatorId { get; set; }

        public int MemberId { get; set; }

        public DialogueMessageDTO LastMessage { get; set; }
    }
}
