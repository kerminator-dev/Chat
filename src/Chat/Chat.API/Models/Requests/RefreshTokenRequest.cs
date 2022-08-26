using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required!")]
        public string RefreshToken { get; set; }
    }
}
