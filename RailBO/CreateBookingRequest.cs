using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class CreateBookingRequest
    {
        public List<Item>? items { get; set; }
        [JsonProperty("correlationid", NullValueHandling = NullValueHandling.Ignore)]
        public string? correlationid { get; set; }
    }
    public class Item
    {
        public List<string>? offerLocations { get; set; }
    }
}
