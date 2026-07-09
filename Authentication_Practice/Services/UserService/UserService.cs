using Authentication_Practice.Dto.Account;
using Authentication_Practice.Models.Account;
using Authentication_Practice.Services.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        // ⭐ نقش‌های ثابت سیستم
        private const string DefaultRole = "User";

        public UserService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // ================= LOGIN =================

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username.ToLower());
            
            if (user == null)
                throw new AppException(
                    "نام کاربری یا رمز عبور اشتباه می باشد",
                     StatusCodes.Status401Unauthorized
                );
            //throw new Exception("نام کاربری یا رمز عبور اشتباه می باشد");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                throw new AppException(
                    "نام کاربری یا رمز عبور اشتباه می باشد",
                     StatusCodes.Status401Unauthorized
                );

            var accessToken = await _tokenService.CreateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                Username = user.UserName!,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };
        }

        // ================= REGISTER =================

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {

            var isEmailExist = await _userManager.FindByEmailAsync(dto.Email);
            //var isUsernameExist = await _userManager.FindByEmailAsync(dto.Username);
            var isUsernameExist = await _userManager.FindByNameAsync(dto.Username);

            if (dto.Email == isEmailExist?.Email)
                throw new InvalidOperationException("کاربری با این ایمیل از قبل وجود دارد");

            if (dto.Username == isUsernameExist?.UserName)
                throw new InvalidOperationException("کاربری با این نام کاربری از قبل وجود دارد");


            var user = new AppUser
            {
                UserName = dto.Username.ToLower(),
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new AppException(
                    "ثبت نام ناموفق بود",
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(e => e.Description)
                );
            }

            //if (!result.Succeeded)
            //    throw new InvalidOperationException("ثبت نام ناموفق بود");

            //    if (!result.Succeeded)
            //        throw new AppException(
            //"ثبت‌نام ناموفق بود",
            //StatusCodes.Status400BadRequest,
            //result.Errors.Select(e => e.Description)
            //);

            // ⭐ اضافه کردن نقش پیش‌فرض (مشتری)
            if (!await _userManager.IsInRoleAsync(user, DefaultRole))
                await _userManager.AddToRoleAsync(user, DefaultRole);

            var token = await _tokenService.CreateToken(user);

            //----- This is new ------

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);

            //------------------------

            return new AuthResponseDto
            {
                Username = user.UserName!,
                AccessToken = token,
                RefreshToken = refreshToken.Token,
            };
        }

        // ================= UPDATE PROFILE =================

        public async Task UpdateUserAsync(string username, UpdateProfileDto dto)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                //throw new Exception("کاربر پیدا نشد");
                throw new AppException(
                    "کاربر پیدا نشد",
                    StatusCodes.Status404NotFound
                );

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Address = dto.Address;
            //user.PostCode = dto.PostCode;
            //user.UserCity = dto.UserCity;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new AppException(
                    "به‌روزرسانی پروفایل ناموفق بود",
                    StatusCodes.Status400BadRequest,
                    result.Errors.Select(e => e.Description)
                );

            //throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // ⭐ شماره تلفن جداگانه
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, dto.PhoneNumber);

                if (!phoneResult.Succeeded)
                    throw new AppException(
                        "شماره تلفن نامعتبر است",
                        StatusCodes.Status400BadRequest,
                        phoneResult.Errors.Select(e => e.Description)
    );
                //throw new Exception(string.Join(", ", phoneResult.Errors.Select(e => e.Description)));
            }
        }

        // ================= GET PROFILE =================

        public async Task<UserProfileDto> GetUserProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new AppException(
        "کاربر پیدا نشد",
        StatusCodes.Status404NotFound
    );

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                Address = user.Address,
                Roles = roles.ToList()
                //PostCode = user.PostCode,
                //UserCity = user.UserCity
            };
        }

        // ================= ADMIN: GET ALL USERS =================

        public async Task<List<UserListItemDto>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Select(u => new UserListItemDto
                {
                    Username = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FullName
                })
                .ToListAsync();
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == token));

            if (user == null)
                throw new AppException(
        "توکن نامعتبر است",
        StatusCodes.Status401Unauthorized
    );
            //throw new Exception("توکن نامعتبر است");

            var refreshToken = user.RefreshTokens.First(t => t.Token == token);

            if (refreshToken.IsExpired || refreshToken.IsRevoked)
                throw new AppException(
        "توکن منقضی شده",
        StatusCodes.Status401Unauthorized
    );
            //throw new Exception("توکن منقضی شده");

            // 🔁 revoke قبلی
            refreshToken.IsRevoked = true;

            // ✨ ساخت جدید
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            var newAccessToken = await _tokenService.CreateToken(user);

            return new AuthResponseDto
            {
                Username = user.UserName!,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(string token)
        {
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x =>
                    x.RefreshTokens.Any(r => r.Token == token));

            if (user == null)
                return;

            var refresh = user.RefreshTokens
                .FirstOrDefault(x => x.Token == token);

            if (refresh == null)
                return;

            refresh.IsRevoked = true;

            await _userManager.UpdateAsync(user);
        }
    }
}

