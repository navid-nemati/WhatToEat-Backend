using Authentication_Practice.Dto.Food;
using Authentication_Practice.Services.FoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _service;

        public FoodController(IFoodService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _service.GetByIdAsync(id);

            return data is null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateFoodDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            return Ok(created);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(Guid Id, UpdateFoodDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(Id ,dto);

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            await _service.DeleteAsync(Id);
            return NoContent();
        }
    }
}
