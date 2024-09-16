using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("PaymentGatewayMode", Schema = "dbo")]
    public class PaymentGatewayModel
    {
        //public int Id { get; set; }
        //public int PGID { get; set; }
        public string? Mode { get; set; }
        public double Charges { get; set; }
        //public bool IsActive { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public float Vat { get; set; }
    }
}
