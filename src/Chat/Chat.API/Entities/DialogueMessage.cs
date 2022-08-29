namespace Chat.API.Entities
{
    public class DialogueMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int DialogueId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
