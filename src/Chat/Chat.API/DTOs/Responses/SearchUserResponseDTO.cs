namespace Chat.API.DTOs.Responses
{
    public class SearchUserResponseDTO
    {
        public ICollection<UserDTO> Users { get; set; }
    }
}
