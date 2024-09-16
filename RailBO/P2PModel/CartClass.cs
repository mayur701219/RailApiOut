using Rail.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.P2PModel
{
    public class CartClass
    {
        public BookingItemsModel? cartModel { get; set; }
        public List<BookingSectorsModel>? SectorList { get; set; }
    }
}
