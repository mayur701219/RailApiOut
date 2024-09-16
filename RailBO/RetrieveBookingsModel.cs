using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    //public class RetrieveBookingsModel
    //{
    //}
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BillingsGross
    {
        public double value { get; set; }
        public string? currency { get; set; }
    }

    public class BillingsNet
    {
        public double value { get; set; }
        public string? currency { get; set; }
    }

    public class Contact
    {
        public string? title { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? emailAddress { get; set; }
    }

    public class Result
    {
        public string? id { get; set; }
        public string? bookingStatus { get; set; }
        public string? bookingReference { get; set; }
        public Contact? contact { get; set; }
        public TotalPrice? totalPrice { get; set; }
        public bool group { get; set; }
        public string? bookingOverviewStatus { get; set; }
        public DateTime bookingCreationDate { get; set; }
        public DateTime bookingModificationDate { get; set; }
        public DateTime? travelDate { get; set; }
        public List<Traveler>? travelers { get; set; }
        public string? region { get; set; }
        public string? user { get; set; }
        public string? agencyName { get; set; }
        public List<string>? marketingCarriers { get; set; }
        public string? meanOfPayment { get; set; }
        public int bookingItemsNumber { get; set; }
    }

    public class RetrieveBookingsModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public List<Result>? results { get; set; }
        public bool hasNextPage { get; set; }
    }

    public class TotalPrice
    {
        public List<BillingsGross>? billingsGross { get; set; }
        public List<BillingsNet>? billingsNet { get; set; }
    }

    //public class Traveler
    //{
    //    public string type { get; set; }
    //}


}
