using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class CreateApiRequest
    {
        public List<CartRequest>? request { get; set; }
    }
    public class CartRequest
    {
        public string? SearchId { get; set; }
        public List<string>? offers { get; set; }
    }
}
