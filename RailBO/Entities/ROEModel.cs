using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("ROE", Schema = "dbo")]
    public class ROEModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FromCur { get; set; }
        public string? ToCur { get; set; }
        public double ROE { get; set; }
        public bool IsActive { get; set; }
    }
}
