using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class RefundReponse
    {
        public string Source { get; set; }
        public string Pax { get; set; }
        public string Pnr { get; set; }
        public string OriginalPrice { get; set; }
        public string RefundAmount { get; set; }
        public string Type { get; set; }
    }

    public class RefundResponseApi
    {
        public List<RefundReponse> refundReponse { get; set; } = new List<RefundReponse>();
        public decimal Totalamount { get; set; }
        public string RefundId { get; set; }
    }
}
