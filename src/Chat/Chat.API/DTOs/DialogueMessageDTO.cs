namespace Chat.API.DTOs
{
    public class DialogueMessageDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
