using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("BookingStatusHistory", Schema = "Rail")]
    public class BookingStatusHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Fk_BookingId { get; set; }
        public string? BookingStatus { get; set; }
        //public DateTime? CreatedDate { get; set; }
    }
}
