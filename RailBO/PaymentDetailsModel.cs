using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class PaymentDetailsModel
    {
        public string? PaymentMode { get; set; }
        public string? PaymentType { get; set; }
        public string? AmountPaidByAgent { get; set; }
        public string? MarkUpOnBookingFee { get; set; }
        public string? BookingFee { get; set; }
        public string? TaxOnBookingFee { get; set; }
        public string? BookingId { get; set; }
    }
}
