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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var data = await _service.GetByIdAsync(id);

            return data is null ? NotFound() : Ok(data);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAsync(
            [FromForm] CreateFoodDto dto,
            CancellationToken cancellationToken)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var created = await _service.CreateAsync(
                dto,
                cancellationToken);

            return Ok(created);

            //return CreatedAtAction(
            //    nameof(GetByIdAsync),
            //    new { id = created.Id },
            //    created);
        }

        [HttpPut("{Id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAsync(
            Guid Id,
            [FromForm] UpdateFoodDto dto,
            CancellationToken cancellationToken)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            await _service.UpdateAsync(Id,
                dto,
                cancellationToken);

            return NoContent();
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteAsync(
            Guid Id,
            CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(
                Id,
                cancellationToken);

            return NoContent();
        }
    }
}
