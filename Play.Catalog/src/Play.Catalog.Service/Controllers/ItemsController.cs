using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
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
        private static readonly List<ItemDTO> items = new()
        {
            new ItemDTO(Guid.NewGuid(),"Potion","Restores a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(),"Antidote","Cures poison affect",7,DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(),"Bronze sword","Deals a amall amount of damage",20,DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDTO> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDTO GetById(Guid id)
        {
            var item = items.Where(item => item.id == id).SingleOrDefault();
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDTO> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDTO(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, item);
        }
    }
}
