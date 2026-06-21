namespace Authentication_Practice.Models.Account
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsRevoked { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string? ReplacedByToken { get; set; }

        
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }
}
