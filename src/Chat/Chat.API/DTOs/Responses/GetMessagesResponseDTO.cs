using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.DTOs.Responses
{
    public class GetMessagesResponseDTO
    {
        public int DialogueId { get; set; }
        public ICollection<DialogueMessageDTO> Messages { get; set; }

        public int Count => Messages.Count;
    }
}
