using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("PaxDetails", Schema = "Rail")]
    public class PaxDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long fk_ItemId { get; set; }
        public string? bookingId { get; set; }
        public string? bookingReference { get; set; }
        public string? bookingItemId { get; set; }
        public string? PaxId { get; set; }
        public string? type { get; set; }
        public bool leadTraveler { get; set; }
        public int age { get; set; }
        public string? emailAddress { get; set; }
        public string? phoneNumber { get; set; }
        public string? title { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? Pan { get; set; }
        public string? ConfirmPan { get; set; }
        public string? ConfirmPanName { get; set; }
        public string? PanName { get; set; }
        public string? Nationality { get; set; }
        public string? countryOfResidence { get; set; }
        public string? CountryCode { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? CardCode { get; set; }
        public string? CardType { get; set; }
        public string? Number { get; set; }
        //public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}
