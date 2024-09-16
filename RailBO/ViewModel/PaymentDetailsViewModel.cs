using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ViewModel
{
    public class PaymentDetailsViewModel
    {
        public string? radio_pay_method { get; set; }
        public string? ModeOfPayment { get; set; }
        public string? pgoptionname { get; set; }
        public string? AmountPaidByAgent { get; set; }
        public string? MarkUpBookingFees { get; set; }
        public string? hdnbookingId { get; set; }
        public string? hdnMainId { get; set; }
        public string? alertmail { get; set; }
        public string? obtc { get; set; }
    }
}
