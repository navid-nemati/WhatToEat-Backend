using Microsoft.AspNetCore.Identity;

namespace Authentication_Practice.Models.Account
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }

        //public string? PhoneNumber { get; set; } identity خودش پراپرتی شماره موبایل داره نمیشه اینجا دوباره تعریف کرد مگه اینکه override بزاری

        public List<RefreshToken> RefreshTokens { get; set; } = new();//new
    }
}
