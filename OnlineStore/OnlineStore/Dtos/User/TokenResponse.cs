namespace OnlineStore.Dtos.User
{
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
