using System.Data;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories.Interfaces;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Item> _itemRepository;

        public ItemController(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemRepository.GetAllAsync()).Select(item => item.AsDto());

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemRepository.FindByIdAsync(id);
            if (item == null)
            {
                return NotFound(new { message = "Item Not Found" });
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateAsync(CreateItemDto createItemDto)
        {
            var newItem = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemRepository.InsertAsync(newItem);

            return CreatedAtAction(nameof(GetByIdAsync), new { Id = newItem.Id }, newItem.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> UpdateAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var foundItem = await _itemRepository.FindByIdAsync(id);
            if (foundItem == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            var item = new Item
            {
                Id = foundItem.Id,
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price,
                CreatedDate = foundItem.CreatedDate
            };

            await _itemRepository.UpdateAsync(item);


            return item.AsDto();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var foundItem = await _itemRepository.FindByIdAsync(id);
            if (foundItem == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            await _itemRepository.RemoveAsync(id);

            return NoContent();
        }
    }

}