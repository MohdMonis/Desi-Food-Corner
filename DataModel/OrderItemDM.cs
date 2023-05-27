using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderItemDM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Required
        public Guid OrderId { get; set; } // Required
        public string OrderName { get; set; } // Required
        public Guid ItemMasterId { get; set; } // Required
        public string ItemMasterName { get; set; } // Required
        public Decimal Price { get; set; } // Required
        public int Quantity { get; set; } // Required
        public Decimal Amount { get; set; }
    }
}
