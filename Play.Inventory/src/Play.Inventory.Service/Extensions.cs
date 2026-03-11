using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDTO AsDTO(this InventoryItem inventoryItem, string name, string description)
        {
            return new InventoryItemDTO(inventoryItem.catalogItemId, inventoryItem.quantity, name, description, inventoryItem.acquiredDate);
        }
    }
}
