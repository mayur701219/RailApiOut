using Newtonsoft.Json;
using Rail.BO.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.P2PModel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TicketsModelResponse
    {
        public string? id { get; set; }
        public string? pointOfSale { get; set; }
        public List<Leg>? legs { get; set; }
        public List<TravelerP2p>? travelers { get; set; }
        public List<Offer>? offers { get; set; }
        public List<Product>? products { get; set; }
        public List<Service>? services { get; set; }
        public List<Highlight>? highlights { get; set; }
    }
    
    public class Amount
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class Billing
    {
        public BillingPrice? billingPrice { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
    }

    public class BillingPrice
    {
        public Amount? amount { get; set; }
    }

    public class CabinClass
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
    }

    public class ComfortCategory
    {
        public string? code { get; set; }
        public string? label { get; set; }
    }

    public class CompatibleHighlight
    {
        public string? legSolution { get; set; }
        public string? strategy { get; set; }
        public List<Highlight>? highlights { get; set; }
    }

    public class Destination
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public string? code { get; set; }
        public string? label { get; set; }
        public string? localLabel { get; set; }
        public string? timezone { get; set; }
    }

    public class Fare:Fare2
    {
        public string? productCode { get; set; }
        public List<string>? segments { get; set; }
        public List<string>? travelers { get; set; }
    }

    public class Fare2
    {
        public string? label { get; set; }
        public Flexibility? flexibility { get; set; }
        public string? conditions { get; set; }
    }

    public class FareOffer
    {
        public string? id { get; set; }
        public List<Fare>? fares { get; set; }
        public Prices? prices { get; set; }
        public DateTime? expiration { get; set; }
        public SupplierReference? supplierReference { get; set; }
    }

    public class Flexibility
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public int level { get; set; }
    }

    public class Highlight
    {
        public string? legSolution { get; set; }
        public string? strategy { get; set; }
        public List<Highlight>? highlights { get; set; }
        public string? comfort { get; set; }
        public List<string>? offers { get; set; }
    }

    public class Leg
    {
        public Origin? origin { get; set; }
        public Destination? destination { get; set; }
        public DateTime? departure { get; set; }
        public bool directOnly { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Solution>? solutions { get; set; }
    }

    public class Offer
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public Prices? prices { get; set; }
        public string? expirationDate { get; set; }
        public string? location { get; set; }
        public List<FareOffer>? fareOffers { get; set; }
        public List<ServiceOffer>? serviceOffers { get; set; }
        public string? legSolution { get; set; }
        public List<string>? compatibleOffers { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
        public Flexibility? flexibility { get; set; }
        public List<TicketingOption>? ticketingOptions { get; set; }
        public List<CompatibleHighlight>? compatibleHighlights { get; set; }
        public string ProductName { get; set; }
        public int? ServiceFeesAppliedId { get; set; }
        public decimal? AgentMarkUpOnBookingFee { get; set; }
    }

    public class Origin
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public string? code { get; set; }
        public string? label { get; set; }
        public string? localLabel { get; set; }
        public string? timezone { get; set; }
    }

    public class PartnerCommission
    {
        public Amount? amount { get; set; }
    }

    public class PaxType
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public Ticket? ticket { get; set; }
    }

    public class Prices
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
        public Ticket? ticket { get; set; }
        public Reservation? reservation { get; set; }
        public Fees? fees { get; set; }
        public Total? total { get; set; }
    }

    public class Product
    {
        public string? type { get; set; }
        public string? code { get; set; }
        public string? label { get; set; }
        public string? marketingCarrier { get; set; }
        public string? supplier { get; set; }
        public PaxType? paxType { get; set; }
        public CabinClass? cabinClass { get; set; }
        public Fare? fare { get; set; }
        public Flexibility? flexibility { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
        public string? conditions { get; set; }
    }

    public class Segment
    {
        public string? id { get; set; }
        public int sequenceNumber { get; set; }
        public Origin? origin { get; set; }
        public Destination? destination { get; set; }
        public DateTime? departure { get; set; }
        public DateTime? arrival { get; set; }
        public string? duration { get; set; }
        public string? operatingCarrier { get; set; }
        public string? marketingCarrier { get; set; }
        public Vehicle? vehicle { get; set; }
        public string? type { get; set; }
        public string? marketingInformation { get; set; }
    }
    public class SegmentConnection
    {
        public Warning? warning { get; set; }
        public string? previousSegmentId { get; set; }
        public string? nextSegmentId { get; set; }
        public string? connectingTime { get; set; }
    }
    public class Selling
    {
        public SellingPrice? sellingPrice { get; set; }
        public SellingPrice? agentSellingPrice { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
        public PartnerCommission? agentCommission { get; set; }
    }

    public class SellingPrice
    {
        public Amount? amount { get; set; }
    }

    public class Service
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public string? label { get; set; }
        public bool included { get; set; }
        public bool additionalService { get; set; }
    }

    public class ServiceOffer
    {
        public string? service { get; set; }
        public List<string>? segments { get; set; }
        public List<string>? travelers { get; set; }
    }

    public class Solution
    {
        public string? id { get; set; }
        public Origin? origin { get; set; }
        public Destination? destination { get; set; }
        public DateTime? departure { get; set; }
        public DateTime? arrival { get; set; }
        public string? duration { get; set; }
        public List<Segment>? segments { get; set; }
        public List<SegmentConnection>? segmentConnections { get; set; }
        public TravelerInformationRequired? travelerInformationRequired { get; set; }
    }

    public class TicketingOption
    {
        public string? code { get; set; }
    }
    public class Ticket
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
    }
    public class TravelerP2p
    {
        public string? id { get; set; }
        public int age { get; set; }
        public string? type { get; set; }
        public List<Cards>? cards { get; set; }
    }

    public class TravelerInformationRequired
    {
        public List<string>? defaultTravelerInformationRequired { get; set; }
        public List<string>? leadTravelerInformationRequired { get; set; }
    }

    public class Vehicle
    {
        public string? type { get; set; }
        public string? reference { get; set; }
        public string? code { get; set; }
    }


}
