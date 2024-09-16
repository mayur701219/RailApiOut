using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.P2PModel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SearchTicketsRequest
    {
        public List<TravelerP2pAge>? travelers { get; set; }
        public List<LegRequest>? legs { get; set; }
    }
    public class TravelerP2pAge
    {
        public int age { get; set; }
        public List<Cards>? cards { get; set; }
    }
    public class Cards
    {
        public string? code { get; set; }
        public string? type { get; set; }
        public string? number { get; set; }

    }
    public class LegRequest
    {
        public OriginRequest? origin { get; set; }
        public DestinationRequest? destination { get; set; }
        public DateTime? departure { get; set; }
        public bool directOnly { get; set; }
    }
    public class OriginRequest
    {
        public string? type { get; set; }
        public string? code { get; set; }
    }
    public class DestinationRequest : OriginRequest
    {

    }
}
