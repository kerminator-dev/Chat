using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class CreateDialogueRequestDTO
    {
        [Required(ErrorMessage = "User is required!")]
        public int TargetUserId { get; set; }
    }
}
