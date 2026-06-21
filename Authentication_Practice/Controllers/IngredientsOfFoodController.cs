using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Services.IngredientsOfFoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsOfFoodController : ControllerBase
    {
        private readonly IIngredientsOfFoodService _service;

        public IngredientsOfFoodController(IIngredientsOfFoodService service)
        {
            _service = service;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAllByFoodIdAsync(Guid Id)
        {
            var data = await _service.GetAllIngredientsOfFoodByFoodIdAync(Id);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateIngredientsOfFoodDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _service.CreateIngredientsOfFoodAsync(dto);

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid Id, UpdateIngredientsOfFoodDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateIngredientsOfFood(Id, dto);

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            await _service.DeleteIngredientsOfFood(Id);
            return NoContent();
        }
    }
}
