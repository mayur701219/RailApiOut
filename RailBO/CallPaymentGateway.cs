using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class CallPaymentGateway
    {
        public string? URLCondition { get; set; }
        public string? PaymentOrderId { get; set; }
        public string? Currency { get; set; }
        public string? PayFirstName { get; set; }
        public string? PayLastName { get; set; }
        public string? PayEmail { get; set; }
        public string? PayMobile { get; set; }
        public string? PayCountry { get; set; }
        public string? PayOrderId { get; set; }
        public string? PayAmount { get; set; }
        public string? ModeOfPaymet { get; set; }
        public string? PaySaltKey { get; set; }
        public string? token { get; set; }
    }
}
