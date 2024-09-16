using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rail.BO.Entities
{
    [Table("BookingItems", Schema = "Rail")]
    public class BookingItemsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long fk_bookingId { get; set; }
        public string? bookingId { get; set; }
        public string? bookingReference { get; set; }
        public string? bookingItemId { get; set; }
        public string? CorrelationId { get; set; }
        public string? Type { get; set; }
        public string? PNR { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public int isDirect { get; set; }
        public bool isRoundTrip { get; set; }
        public bool isInBound { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public string? Duration { get; set; }
        public string? Country { get; set; }
        public string? Comfort { get; set; }
        public string? Flexibility { get; set; }
        public string? numberOfTravelDays { get; set; }
        public DateTime? expirationDate { get; set; }
        public DateTime? activationPeriodStart { get; set; }
        public DateTime? activationPeriodEnd { get; set; }
        public DateTime? validityPeriodStart { get; set; }
        public DateTime? validityPeriodEnd { get; set; }
        public string? Title { get; set; }
        public string? Conditions { get; set; }
        public string? PaxDetails { get; set; }
        public string? PaxPricing { get; set; }
        public string? Currency { get; set; }
        public string? AgentCurrency { get; set; }
        public decimal RiyaCommission { get; set; }
        public decimal RiyaAmount { get; set; }
        public decimal AgentCommission { get; set; }
        public decimal AgentAmount { get; set; }
        public decimal ReservationFee { get; set; }
        public double ROE { get; set; }
        public string? roeMarkup { get; set; }
        public double FinalROE { get; set; }
        public double SupplierToInrROE { get; set; }
        public string? SupplierToInrMarkup { get; set; }
        public double SupplierToInrFinal { get; set; }
        public double AgentToInrROE { get; set; }
        public string? AgentToInrMarkup { get; set; }
        public double AgentToInrFinal { get; set; }
        public string? Location { get; set; }
        public string? ProductName { get; set; }
        public string? SupplierId { get; set; }
        public long AgentId { get; set; }
        public long RiyaUserId { get; set; }
        public string? DeviceId { get; set; }
        public string? Status { get; set; }
        public string? Response { get; set; }
        public long ParentId { get; set; }
        //public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        public int? ServiceFeesAppliedId { get; set; }
        ////////////////////////////////////////////////////
        [NotMapped]
        public double CurrRoe { get; set; }
    }
}
