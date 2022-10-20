using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class UpdateColorRequestDTO
    {
        [Required(ErrorMessage = "Color is required!")]
        public int Color { get; set; }
    }
}
