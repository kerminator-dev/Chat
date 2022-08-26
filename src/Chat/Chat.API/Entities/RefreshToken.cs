namespace Chat.API.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}
