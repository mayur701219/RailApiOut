using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("EraLogs", Schema = "Rail")]
    public class EraLogsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long fk_bookingId { get; set; }
        public string? Url { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? Stage { get; set; }
    }
}
