using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class LogoutRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
