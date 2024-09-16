using Rail.BO.P2PModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class ErrorResponse
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public List<string>? details { get; set; }
        public string? type { get; set; }
    }
    public class OfferErrorResponse
    {
        public string? id { get; set; }
        public List<ErrorResponse>? errors { get; set; }
        public string? pointOfSale { get; set; }
        public List<Leg>? legs { get; set; }
        public List<TravelerP2p>? travelers { get; set; }
    }
}
