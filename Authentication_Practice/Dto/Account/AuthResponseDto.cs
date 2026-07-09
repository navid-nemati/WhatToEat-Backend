namespace Authentication_Practice.Dto.Account
{
    public class AuthResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
