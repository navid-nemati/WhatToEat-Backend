using Authentication_Practice.Models.Account;

namespace Authentication_Practice.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser appUser);
        RefreshToken GenerateRefreshToken();
    }
}
