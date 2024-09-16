using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class CommonResponseModel
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }
}
