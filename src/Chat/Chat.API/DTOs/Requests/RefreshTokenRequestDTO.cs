using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Refresh token is required!")]
        public string RefreshToken { get; set; }
    }
}
