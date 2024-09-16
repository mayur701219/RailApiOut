using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("Tokens", Schema = "Rail")]
    public class TokensModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? token { get; set; }
        public DateTime? expiration { get; set; }
    }
}
