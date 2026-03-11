using Play.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Entities
{
    public class InventoryItem : IEntity
    {
        public Guid id { get; set; }
        public Guid userId { get; set; }
        public Guid catalogItemId { get; set; }
        public int quantity { get; set; }
        public DateTimeOffset acquiredDate { get; set; }

    }
}
