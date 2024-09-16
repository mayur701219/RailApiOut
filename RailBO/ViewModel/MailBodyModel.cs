using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ViewModel
{
    public class MailBodyModel
    {
        public string? Body { get; set; }
        public string? BookingReference { get; set; }
        public Stream? attachment { get; set; }
    }
}
