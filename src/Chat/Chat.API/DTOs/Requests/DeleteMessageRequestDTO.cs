using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class DeleteMessagesRequestDTO
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }

        [Required(ErrorMessage = "Messages are required!")]
        [MinLength(1)]
        public List<int> MessageIds { get; set; }
    }
}
