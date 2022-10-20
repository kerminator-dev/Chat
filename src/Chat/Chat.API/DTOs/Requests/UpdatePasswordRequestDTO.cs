using System.ComponentModel.DataAnnotations;

namespace Chat.API.DTOs.Requests
{
    public class UpdatePasswordRequestDTO
    {
        [Required(ErrorMessage = "Old password is required!")]
        [MinLength(6, ErrorMessage = "Min password length is 6 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$", ErrorMessage = "Password may contains digits and letters. Length of password is 3-20 characters!")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required!")]
        [MinLength(6, ErrorMessage = "Min password length is 6 characters!")]
        [MaxLength(20, ErrorMessage = "Max password length is 20 characters!")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$", ErrorMessage = "Password may contains digits and letters. Length of password is 3-20 characters!")]
        public string NewPassword { get; set; }
    }
}
