using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("BookingSectors", Schema = "Rail")]
    public class BookingSectorsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long fk_bookingitemId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public string? Duration { get; set; }
        public int SequenceNumber { get; set; }
        public bool isRoundTrip { get; set; }
        public bool isInBound { get; set; }
        //public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}
