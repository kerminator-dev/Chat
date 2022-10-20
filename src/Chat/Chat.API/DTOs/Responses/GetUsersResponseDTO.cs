namespace Chat.API.DTOs.Responses
{
    public class GetUsersResponseDTO
    {
        public ICollection<UserDTO> Users { get; set; }

        public int Count => Users.Count;
    }
}
