namespace Chat.API.Models.Responses
{
    public class SearchUserResponse
    {
        public ICollection<UserDTO> Users { get; set; }
    }
}
