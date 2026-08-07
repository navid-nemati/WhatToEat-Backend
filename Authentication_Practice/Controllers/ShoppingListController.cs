using Authentication_Practice.Dto.ShoppingListItem;
using Authentication_Practice.Services.ShoppingListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Practice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _service;

        public ShoppingListController(IShoppingListService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task CreateAsync(AddShoppingListDto dto)
        {
            await _service.AddAsync(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _service.GetAllAsync();

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateShoppingListDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _service.DeleteAllAsync();
            return NoContent();
        }

        [HttpDelete("purchased")]
        public async Task<IActionResult> DeletePurchasedAsync()
        {
            await _service.DeletePurchasedAsync();
            return NoContent();
        }
    }
}
