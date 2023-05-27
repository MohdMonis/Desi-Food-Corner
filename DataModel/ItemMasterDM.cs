using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ItemMasterDM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Required
        public Guid CategoryId { get; set; } // Required
        public string CategoryName { get; set; } // Required
        public string Description { get; set; }
        public decimal Price { get; set; } // Required
        public int TypeValue { get; set; } // Required
        public string TypeText { get; set; } // Required
    }
}
