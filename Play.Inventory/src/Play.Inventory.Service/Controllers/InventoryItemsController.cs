using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("inventoryItems")]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> inventoryItemsRepository;
        private readonly CatalogClient _catalogClient;
        public InventoryItemsController(IRepository<InventoryItem> inventoryItemsRepository, CatalogClient catalogClient)
        {
            this.inventoryItemsRepository = inventoryItemsRepository;
            this._catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }
            var catalogItems = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await inventoryItemsRepository.GetAllAsync(item => item.userId == userId);
            var inventoryItemsDTOs = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogMatch = catalogItems.Single(catalogItem => catalogItem.id == inventoryItem.catalogItemId);
                return inventoryItem.AsDTO(catalogMatch.Name, catalogMatch.Description);
            });
            return Ok(inventoryItemsDTOs);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDTO grantItemDTO)
        {
            var currentInventoryItem = await inventoryItemsRepository.GetAsync(inventoryItem => inventoryItem.userId == grantItemDTO.userId &&
            inventoryItem.catalogItemId == grantItemDTO.catalogItemId);
            if (currentInventoryItem == null)
            {
                var inventoryItem = new InventoryItem
                {
                    catalogItemId = grantItemDTO.catalogItemId,
                    quantity = grantItemDTO.quantity,
                    userId = grantItemDTO.userId,
                    acquiredDate = DateTimeOffset.UtcNow,
                };
                await inventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                currentInventoryItem.quantity += grantItemDTO.quantity;
                await inventoryItemsRepository.UpdateAsync(currentInventoryItem);
            }

            return Ok();
        }
    }
}
