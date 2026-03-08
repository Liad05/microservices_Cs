using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;
using System.Runtime.CompilerServices;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO(Guid.NewGuid(), item.Name, item.Description, item.Price, item.CreatedDate);
        }

        public static Item AsEntity(this CreateItemDto createItemDTO)
        {
            return new Item
            {
                Name = createItemDTO.Name,
                Description = createItemDTO.Description,
                Price = createItemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        public static Item AsEntity(this UpdateItemDto updateItemDTO)
        {
            return new Item
            {
                Name = updateItemDTO.Name,
                Description = updateItemDTO.Description,
                Price = updateItemDTO.Price,
            };
        }

    }
}