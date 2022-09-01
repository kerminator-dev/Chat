namespace Chat.API.Models.Responses
{
    public class GetUsersResponse
    {
        public ICollection<UserDTO> Users { get; set; }

        public int Count => Users.Count;
    }
}
