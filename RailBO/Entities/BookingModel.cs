using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("Bookings", Schema = "Rail")]
    public class BookingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? BookingId { get; set; }
        public string? BookingReference { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime? creationDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public string? Currency { get; set; }
        public string? AgentCurrency { get; set; }
        public decimal totalbillingsGross { get; set; }
        public decimal totalbillingsNet { get; set; }
        public string? bookingStatus { get; set; }
        public string? overviewStatus { get; set; }
        public long AgentId { get; set; }
        public long RiyaUserId { get; set; }
        public decimal AmountPaidbyAgent { get; set; }
        public decimal MarkUpOnBookingFee { get; set; }
        public decimal ReservationFee { get; set; }
        public decimal BookingFee { get; set; }
        public decimal TaxOnBookingFee { get; set; }
        public string? BFCurrency { get; set; }
        public double BFROE { get; set; }
        public string? BFroeMarkup { get; set; }
        public double BFFinalROE { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentMode { get; set; }
        public double ROE { get; set; }
        public string? Supplier { get; set; }
        public bool IsRetrievePnr { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        //public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        public string? OBTCNo { get; set; }
        public string? RiyaPnr { get; set; }
        public double BFToInrROE { get; set; }
        public string? BFToInrMarkup { get; set; }
        public double BFToInrFinal { get; set; }
        public decimal AmountPaidbyAgentInr { get; set; }
        public DateTime? BookingDate { get; set; }
        public bool IsApiBooking { get; set; }
    }
}
