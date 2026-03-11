using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;
using Play.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;
        private static int requestsCounter = 0;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAsync()
        {
            requestsCounter++;
            Console.WriteLine($"Request {requestsCounter}: starting...");
            if (requestsCounter <= 2)
            {
                Console.WriteLine($"Request {requestsCounter} delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if (requestsCounter <= 4)
            {
                Console.WriteLine($"Request {requestsCounter}: 500 (internal server error) ...");
                return StatusCode(500);
            }
            Console.WriteLine($"Request {requestsCounter}: 200 (ok) ...");
            return Ok((await itemsRepository.GetAllAsync()).Select(item => item.AsDTO()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetByIdAsync(Guid id)
        {
            var item = (await itemsRepository.GetByIdAsync(id)).AsDTO();
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDto createItemDto)
        {
            var item = createItemDto.AsEntity();
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto changeItemDto)
        {
            var isValidId = (await itemsRepository.GetByIdAsync(id)) != null;
            if (!isValidId)
            {
                return NotFound();
            }
            await itemsRepository.UpdateAsync(changeItemDto.AsEntity());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var itemToDelete = await itemsRepository.GetByIdAsync(id);
            if (itemToDelete == null) return NotFound();
            await itemsRepository.RemoveAsync(id);
            return NoContent();
        }
    }
}
