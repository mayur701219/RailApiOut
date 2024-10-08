using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    public class SearchRequestModel
    {
        //public string? AgentId { get; set; }
        //public string? correlation { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public string? journeyStartDate { get; set; }
        public string? journeyStartTime { get; set; }
        public CommonLocation? From { get; set; }
        public CommonLocation? To { get; set; }
        [Required]
        public bool? isRoundTrip { get; set; }
        public string? returnDate { get; set; }
        public string? returnTime { get; set; }      
        public List<Traveler>? travelers { get; set; }
        public bool isFamilyCard { get; set; }
    }
    public class CommonLocation : CodeLabelModel
    {
        public string? type { get; set; }
    }
}
