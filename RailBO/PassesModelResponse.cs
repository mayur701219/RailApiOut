using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PassesModelResponse
    {
        public string? id { get; set; }
        public List<Warning>? warnings { get; set; }
        public Place? place { get; set; }
        public string? validityStartDate { get; set; }
        public List<TravelerResponse>? travelers { get; set; }
        public List<Offer>? offers { get; set; }
        public List<Product>? products { get; set; }
    }

    public class ActivationPeriod
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
    }

    public class Amount
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class Billing
    {
        public BillingPrice? billingPrice { get; set; }
        public NetAmount? netAmount { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
    }

    public class BillingPrice
    {
        public Amount? amount { get; set; }
    }

    public class ComfortCategory : CodeLabelModel { }

    public class Condition
    {
        public string? condition { get; set; }
        public string? type { get; set; }
    }

    public class Fare
    {
        public int numberOfTravelDays { get; set; }
        public string? validityDuration { get; set; }
        public string? activationDuration { get; set; }
    }

    public class Offer
    {
        public string? id { get; set; }
        public string? validityDuration { get; set; }
        public string? label { get; set; }
        public Prices? prices { get; set; }
        public int numberOfTravelDays { get; set; }
        public string? activationDuration { get; set; }
        public ActivationPeriod? activationPeriod { get; set; }
        public ActivationPeriod? validityPeriod { get; set; }
        public List<Condition>? conditions { get; set; }
        public List<TravelerPassOffer>? travelerPassOffers { get; set; }
        public List<string>? tags { get; set; }
        public TravelClass? travelClass { get; set; }
        public TravelerInformationRequired? travelerInformationRequired { get; set; }
        public DateTime expirationDate { get; set; }
        public string? location { get; set; }
        public string? marketingCarrier { get; set; }
        public string? type { get; set; }
        public int? ServiceFeesAppliedId { get; set; }
        public decimal? AgentMarkUpOnBookingFee { get; set; }

    }

    public class PartnerCommission
    {
        public PartnerCommission()
        {
            amount = new Amount();
        }
        public Amount? amount { get; set; }
    }

    public class PassTravelClass
    {
        public string? code { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
    }

    public class Place : CodeLabelModel { }

    public class Prices
    {
           
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }

        //public Selling? selling { get; set; }
        //public Selling? selling;
        //public Selling? GetSelling()
        //{
        //     return new Selling
        //        {
        //            sellingPrice = new SellingPrice
        //            {
        //                amount = billings[0]?.billingPrice?.amount
        //            },
        //            partnerCommission = new PartnerCommission
        //            {
        //                amount = billings[0]?.partnerCommission?.amount
        //            },
        //            agentCommission = new PartnerCommission(),
        //            agentSellingPrice = new SellingPrice()
        //        };
        //}
        //public void SetSelling(Selling? value)
        //{
        //    selling = value;
        //}

        //public Selling? Selling
        //{
        //    get
        //    {
        //        selling = new Selling
        //        {
        //            sellingPrice = new SellingPrice
        //            {
        //                amount = billings[0].billingPrice.amount
        //            },
        //            partnerCommission = new PartnerCommission
        //            {
        //                amount = billings[0].partnerCommission.amount
        //            },
        //            agentCommission = new PartnerCommission(),
        //            agentSellingPrice = new SellingPrice()
        //        };
        //        //Selling = selling;
        //        return selling;
        //    }
        //    set {  = value; }
        //}
    }

    public class Product : CodeLabelModel
    {
        public Fare? fare { get; set; }
        public string? supplier { get; set; }
        public string? marketingCarrier { get; set; }
        public List<string>? places { get; set; }
        public string? type { get; set; }
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

    public class TravelClass
    {
        public string? code { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
    }

    public class TravelerInformationRequired
    {
        public List<string>? defaultTravelerInformationRequired { get; set; }
        public List<string>? leadTravelerInformationRequired { get; set; }
    }

    public class TravelerPassOffer
    {
        public Prices? prices { get; set; }
        public string? travelerId { get; set; }
        public string? productCode { get; set; }
        public PassTravelClass? passTravelClass { get; set; }
    }

    public class Warning : CodeLabelModel
    {
        public List<string>? details { get; set; }
        public string? type { get; set; }
    }


}
