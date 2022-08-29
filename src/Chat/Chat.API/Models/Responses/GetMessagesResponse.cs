using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Models.Responses
{
    public class GetMessagesResponse
    {
        public int DialogueId { get; set; }
        public ICollection<DialogueMessageDTO> Messages { get; set; }

        public int Count => Messages.Count;
    }
}
