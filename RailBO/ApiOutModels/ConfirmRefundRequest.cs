using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public  class ConfirmRefundRequest
    {
        public string bookingId { get; set; }
        public string refundId { get; set; }
        public string totalRefund { get; set; }
    }
}
