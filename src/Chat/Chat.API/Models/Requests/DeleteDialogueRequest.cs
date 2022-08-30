using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class DeleteDialogueRequest
    {
        [Required]
        public int DialogueId { get; set; }
    }
}
