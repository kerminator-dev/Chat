using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.DTOs.Responses
{
    public class GetDialoguesResponseDTO
    {
        public int UserId { get; set; }

        public ICollection<DialogueWithLastMessageDTO> Dialogues { get; set; }

        public int Count => Dialogues == null ? 0 : Dialogues.Count;
    }
}
