using Chat.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Chat.API.Models.Requests
{
    public class CreateConversationRequest
    {
        [Required(ErrorMessage = "Title is required!")]
        [MinLength(1, ErrorMessage = "Min title length should be 1 symbol!")]
        [MaxLength(20, ErrorMessage = "Max title length should be 20 symbols!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Conversation members are required!")]
        [MinLength(1, ErrorMessage = "Minimal cnversation members is 1 user except creator!")]      // + 1 - Текущий пользователь
        [MaxLength(4, ErrorMessage = "Maximal conversations memebers is 4 users except creator!")]  // + 1 - Текущий пользователь
        public ICollection<int> ConversationMembersIds { get; set; }
    }
}
