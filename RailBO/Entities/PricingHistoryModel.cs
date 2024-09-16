using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("PricingHistory", Schema = "Rail")]
    public class PricingHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Fk_BookingId { get; set; }
        public long Fk_ItemId { get; set; }
        public string? Currency { get; set; }
        public string? AgentCurrency { get; set; }
        public decimal RiyaAmount { get; set; }
        public decimal RiyaCommission { get; set; }
        public decimal AgentAmount { get; set; }
        public decimal AgentCommission { get; set; }
        public double ROE { get; set; }
        public string? roeMarkup { get; set; }
        public double FinalROE { get; set; }
        public decimal ReturnRatio { get; set; }
        public string? OperationType { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
