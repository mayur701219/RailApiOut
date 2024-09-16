using Rail.BO.P2PModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class UpdateTravelerDetailsRequest
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public bool leadTraveler { get; set; }
        public int age { get; set; }
        public string? emailAddress { get; set; }
        public string? phoneNumber { get; set; }
        public string? title { get; set; }
        public string? lastName { get; set; }
        public string? firstName { get; set; }
        public string? dateOfBirth { get; set; }
        public string? countryOfResidence { get; set; }
        public List<Cards>? cards { get; set; }

        public TravelerDocument? travelerDocument { get; set; }
    }
    public class TravelerDocument
    {
        public string? countryCode { get; set; }
        public string? documentNumber { get; set; }
        public string? type { get; set; }
        public string? expirationDate { get; set; }
        public bool validDocument { get; set; }
    }
    public class ValueDocument
    {
        public string? id { get; set; }
        public string? url { get; set; }
        public int size { get; set; }
    }
    public class UpdateTravelerRequest
    {
        public string? bookingId { get; set; }
        public string? itemId { get; set; }
        public string? correlationid { get; set; }
        public List<UpdateTravelerDetailsRequest>? requests { get; set; }
    }
}
