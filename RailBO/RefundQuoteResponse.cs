using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class RefundQuoteResponse
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public List<EraBkItem>? items { get; set; }
        public BalancePrices? balancePrices { get; set; }
        public PointOfSale? pointOfSale { get; set; }
        public DateTime? operationDateTime { get; set; }
        public DateTime? expirationDate { get; set; }
        public string? refundType { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EraAgency
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? email { get; set; }
        public bool primary { get; set; }
        public BillingAddress? billingAddress { get; set; }
    }

    public class AmountsToDisplay
    {
        public Amount? amount { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
    }

    public class AuthorizedProducts
    {
        public List<string>? suppliers { get; set; }
        public List<string>? marketingCarriers { get; set; }
    }

    public class BalancePrices
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
        public List<AmountsToDisplay>? amountsToDisplay { get; set; }
        public ReversedPrices? reversedPrices { get; set; }
        public Total? total { get; set; }
        public ReversedTotal? reversedTotal { get; set; }
    }

    public class BillingAddress
    {
        public string? addressLine1 { get; set; }
        public string? countryCode { get; set; }
        public string? country { get; set; }
        public string? zipCode { get; set; }
        public string? city { get; set; }
    }

    public class EraBkItem
    {
        public string? id { get; set; }
        public PenaltyPrices? penaltyPrices { get; set; }
        public ReversedPrices? reversedPrices { get; set; }
        public BalancePrices? balancePrices { get; set; }
        public decimal carrierRefundRatio { get; set; }
        public bool voiding { get; set; }
        public bool delayed { get; set; }
        public bool fraudDetected { get; set; }
        public string? refundType { get; set; }
    }

    public class KeyAccount
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
    }

    public class NetAmount
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class Payment
    {
        public List<string>? authorizedPaymentMethods { get; set; }
    }

    public class PenaltyPrices
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
        public List<AmountsToDisplay>? amountsToDisplay { get; set; }
    }

    public class PointOfSale
    {
        public string? code { get; set; }
        public Profile? profile { get; set; }
        public string? entity { get; set; }
    }

    public class Profile
    {
        public string? marketType { get; set; }
        public string? salesChannel { get; set; }
        public string? zone { get; set; }
        public string? country { get; set; }
        public string? city { get; set; }
        public AuthorizedProducts? authorizedProducts { get; set; }
        public Payment? payment { get; set; }
        public EraAgency? agency { get; set; }
        public KeyAccount? keyAccount { get; set; }
        public string? timezone { get; set; }
        public string? defaultPriceToDisplay { get; set; }
        public bool groupBookingAllowed { get; set; }
        public string? billingMethod { get; set; }
    }

    public class ReversedPrices
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
        public Selling? agentSellingPrice { get; set; }
        public List<AmountsToDisplay>? amountsToDisplay { get; set; }
    }

    public class ReversedTotal
    {
        public List<BillingsGross>? billingsGross { get; set; }
        public List<BillingsNet>? billingsNet { get; set; }
    }

    public class Total
    {
        public List<BillingsGross>? billingsGross { get; set; }
        public List<BillingsNet>? billingsNet { get; set; }
    }


}
