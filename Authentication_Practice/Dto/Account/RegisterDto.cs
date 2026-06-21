using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Account
{
    public class RegisterDto
    {

        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [MinLength(3, ErrorMessage = "نام کاربری حداقل باید 3 کاراکتر باشد")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نمی باشد")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; }
    }
}
