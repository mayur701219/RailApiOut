using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class RefundQuoteRequestVM : RefundQuoteRequest
    {
        public string? bookingId { get; set; }
        public long pk_bookingId { get; set; }
    }

    public class RefundQuoteRequest
    {
        public string? type { get; set; }
        public List<ItemRefund>? items { get; set; }
    }
    public class Leg
    {
        public string? id { get; set; }
    }
    public class CancellableItem
    {
        public string? id { get; set; }
    }
    public class ItemRefund
    {
        public string? id { get; set; }
        public List<Leg>? legs { get; set; }
        public List<CancellableItem>? cancellableItems { get; set; }
    }
}
