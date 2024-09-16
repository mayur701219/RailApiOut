using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("LOGOs", Schema = "Rail")]
    public class RailLOGOs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Supplier { get; set; }
        public string? marketingInformation { get; set; }
        public string? marketingCarrier { get; set; }
        public string? operationCarrier { get; set; }
        public string? correspondingValue { get; set; }
    }
}
