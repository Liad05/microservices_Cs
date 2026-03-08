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

        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetAsync()
        {
            return (await itemsRepository.GetAllAsync()).Select(item => item.AsDTO());
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
