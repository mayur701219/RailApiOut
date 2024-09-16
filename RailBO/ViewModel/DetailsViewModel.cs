using Rail.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ViewModel
{
    public class DetailsViewModel
    {
        public BookingModel? bookingModel { get; set; }
        public IEnumerable<BookingItemsModel>? bookingItems { get; set; }
        public IEnumerable<PaxDetailModel>? paxDetails { get; set; }
        public CurrentAgent? Agent { get; set; }
        public string ROE { get; set; }
        public string? SelfBalanceAccess { get; set; }
    }
}
