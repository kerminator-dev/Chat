using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class GetUsersRequest
    {
        [Required(ErrorMessage = "Users are required!")]
        [MinLength(1, ErrorMessage = "Min required count of users is 1!")]
        [MaxLength(50, ErrorMessage = "Max required count of users is 50!")]
        public ICollection<int> UserIds { get; set; }
    }
}
