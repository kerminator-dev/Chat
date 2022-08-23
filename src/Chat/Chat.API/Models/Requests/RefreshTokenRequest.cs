using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
