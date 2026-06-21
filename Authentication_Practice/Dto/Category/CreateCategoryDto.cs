using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Category
{
    public class CreateCategoryDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;
    }
}
