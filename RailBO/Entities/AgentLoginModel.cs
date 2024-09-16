using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("AgentLogin", Schema = "dbo")]
    public class AgentLoginModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? MobileNumber { get; set; }
    }
}
