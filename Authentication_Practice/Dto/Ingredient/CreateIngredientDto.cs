using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Ingredient
{
    public class CreateIngredientDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "نام حداقل باید 2 کاراکتر باشد")]
        [MaxLength(50, ErrorMessage = "نام حداکثر می تواند 50 کاراکتر باشد")]
        public string Name { get; set; } = string.Empty;
    }
}
