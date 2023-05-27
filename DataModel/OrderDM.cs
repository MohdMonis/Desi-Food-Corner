using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderDM
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; } // Required
        public string Name { get; set; } // Required
        public Guid CustomerId { get; set; } // Required
        public string CustomerName { get; set; } // Required
        public DateTime DispatchedOn { get; set; }
        public Guid DispatchedById { get; set; } // Required
        public string DispatchedByName { get; set; } // Required
        public DateTime DeliveredOn { get; set; }
        public Guid DeliveredById { get; set; } // Required
        public string DeliveredByName { get; set; } // Required
        public int StatusReasonValue { get; set; } // Required
        public string StatusReasonText { get; set; } // Required
        public string ReasonForCancellation { get; set; }
        public string ContactNo { get; set; } // Required
        public string Address { get; set; }
        public string State { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public int PaymentModeValue { get; set; }
        public string PaymentModeText { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal GST { get; set; }
        public Decimal TotalAmount { get; set; }

    }
}
