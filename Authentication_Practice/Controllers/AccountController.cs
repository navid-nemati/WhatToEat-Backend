using Authentication_Practice.Dto.Account;
using Authentication_Practice.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);

            SetAuthCookies(result);

            //return Ok(result);

            return Ok(new
            {
                username = result.Username
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            SetAuthCookies(result);

            return Ok(new
            {
                username = result.Username
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = await _userService.GetUserProfileAsync(username);

            return Ok(user);
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            await _userService.UpdateUserAsync(username, dto);

            return Ok(new { message = "پروفایل با موفقیت ویرایش شد" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRefreshRequest dto)
        {
            var result = await _userService.RefreshTokenAsync(dto.RefreshToken);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _userService.LogoutAsync(refreshToken);
            }

            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("accessToken");

            return Ok(new
            {
                message = "با موفقیت خارج شدید بای بای 👋"
            });
        }

        private void SetAuthCookies(AuthResponseDto auth)
        {
            Response.Cookies.Append(
                "accessToken",
                auth.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                }
                );

            Response.Cookies.Append(
                "refreshToken",
                auth.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                }
                );
        }

        
    }
}
