using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class RefundQuoteReqeust
    {
        public string bookingId { get; set; }
        public List<string> bookingitemId { get; set; }
    }
}
