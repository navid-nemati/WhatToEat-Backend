using Authentication_Practice.Dto.Ingredient;
using Authentication_Practice.Models.Food;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Food
{
    public class UpdateFoodDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "نام حداقل باید 2 کاراکتر باشد")]
        [MaxLength(50, ErrorMessage = "نام حداکثر می تواند 50 کاراکتر باشد")]
        public string Name { get; set; } = string.Empty;

        public string? Recipe { get; set; }

        [Required(ErrorMessage = "لطفا یک دسته بندی انتخاب کنید")]
        public Guid CategoryId { get; set; }

        public IFormFile? Image { get; set; }
        public bool RemoveImage { get; set; }

//        تصویر جدید فرستاده شود → تصویر قبلی جایگزین شود
//تصویر جدید ارسال نشود → تصویر قبلی باقی بماند
//RemoveImage = true → تصویر قبلی حذف شود
    }
}
