using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ViewModel
{
    public class ROEViewModel
    {
        public List<RoeDatum>? roeData { get; set; }
        public bool isSuccess { get; set; }
        public string? errorMessage { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RoeDatum
    {
        public double roe { get; set; }
        public string? markup { get; set; }
        public double finalROE { get; set; }
    }
}
//{ "ROEData":[{ "ROE":"90.6388659297","Markup":"2 INR","FinalROE":"92.6388659297"}],"IsSuccess":true,"ErrorMessage":null}