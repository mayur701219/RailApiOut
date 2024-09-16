using Newtonsoft.Json;
using Rail.BO.P2PModel;

namespace Rail.BO
{
    public class SearchPassesRequest
    {
        public Place? place { get; set; }
        public List<Traveler>? travelers { get; set; }
        public string? validityStartDate { get; set; }
    }

    public class Traveler
    {
        public int age { get; set; }
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        //public string? id { get; set; }
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        //public string? type { get; set; }
        public List<Cards>? cards { get; set; }
    }
    public class TravelerResponse
    {
        public int age { get; set; }
        public string? id { get; set; }
        public string? type { get; set; }
    }

    public class Cards
    {
        public string? code { get; set; }
        public string? type { get; set; }
        public string? number { get; set; }

    }
}