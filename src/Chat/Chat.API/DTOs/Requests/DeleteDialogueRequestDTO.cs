using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class DeleteDialogueRequestDTO
    {
        [Required(ErrorMessage = "Dialogue is required!")]
        public int DialogueId { get; set; }
    }
}
