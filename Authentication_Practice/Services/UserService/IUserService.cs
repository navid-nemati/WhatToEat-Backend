using Authentication_Practice.Dto.Account;

namespace Authentication_Practice.Services.UserService
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task UpdateUserAsync(string username, UpdateProfileDto dto);
        Task<UserProfileDto> GetUserProfileAsync(string username);
        Task<List<UserListItemDto>> GetAllUsersAsync();
        Task<AuthResponseDto> RefreshTokenAsync(string token);
    }
}
