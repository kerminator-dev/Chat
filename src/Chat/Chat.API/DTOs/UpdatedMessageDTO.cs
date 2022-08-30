using Chat.API.Entities;

namespace Chat.API.DTOs
{
    public class UpdatedMessageDTO
    {
        public int InitiatorId { get; set; }
        public DialogueMessage NewMessage { get; set; }
    }
}
