using Authentication_Practice.Dto.Category;
using Authentication_Practice.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var category = await _categoryService.GetAllAsync();
        //    return Ok(category);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterCatgoryDto filter)
        {
            var data = await _categoryService.FilterCategoriesAsync(filter);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _categoryService.CreateAsync(dto);

            //return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryDto category)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            await _categoryService.UpdateAsync(id, category);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            await _categoryService.DeleteAsync(Id);
            return NoContent();
        }
    }
}
