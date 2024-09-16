using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class BookingSummaryModel
    {
        public string BookingId { get; set; }
        public string BookingReference { get; set; }
        public decimal TotalBillingsGross { get; set; }
        public string Currency { get; set; }
        public string AgentCurrency { get; set; }
        public string BookingStatus { get; set; }
        public string OverviewStatus { get; set; }
        public long AgentId { get; set; }
        public long RiyaUserId { get; set; }
        public List<BookingItemSummaryModel> BookingItems { get; set; }
    }

    public class BookingItemSummaryModel
    {
        public string BookingItemId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public bool IsDirect { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public List<PaxDetailSummaryModel> PaxDetails { get; set; }
    }

    public class PaxDetailSummaryModel
    {
        public string PaxId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
