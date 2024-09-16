using Rail.BO.P2PModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Bookings
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Accommodation
    {
        public string? seatNumber { get; set; }
        public string? coachNumber { get; set; }
        public string? segment { get; set; }
        public string? traveler { get; set; }
    }
    public class ActivationPeriod
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
    }

    public class ValidityPeriod
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
    }

    public class Agency
    {
        public string? agencyName { get; set; }
        public string? emailAddress { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? email { get; set; }
        public bool primary { get; set; }
    }

    public class Amount
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class Applicability
    {
        public string? origin { get; set; }
        public string? @event { get; set; }
        public string? level { get; set; }
    }

    public class Attachment
    {
        public string? name { get; set; }
        public string? publicUrl { get; set; }
        public int size { get; set; }
        public bool readOnly { get; set; }
    }

    public class AuthorizedProducts
    {
        public List<string>? suppliers { get; set; }
        public List<string>? marketingCarriers { get; set; }
    }

    public class Billing
    {
        public BillingPrice? billingPrice { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
        public string? meansOfPayment { get; set; }
        public BuyerDetails? buyerDetails { get; set; }
    }

    public class BillingPrice
    {
        public Amount? amount { get; set; }
        public NetAmount? netAmount { get; set; }
    }

    public class BillingsGross
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class BillingsNet
    {
        public decimal value { get; set; }
        public string? currency { get; set; }
    }

    public class BookingItem
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public string? bookingItemStatus { get; set; }
        public List<string>? offerLocations { get; set; }
        public CancelationEligibility? cancelationEligibility { get; set; }
        public DateTime expirationDate { get; set; }
        public SupplierOrder? supplierOrder { get; set; }
        public List<Products>? products { get; set; }
        public List<ValueDocument>? valueDocuments { get; set; }
        public List<TicketingOption>? ticketingOptions { get; set; }
        public TicketingOptionSelected? ticketingOptionSelected { get; set; }
        public TravelerInformationRequired? travelerInformationRequired { get; set; }
        public List<Ticket>? tickets { get; set; }
        public Prices? prices { get; set; }
        public List<LegResponse>? legs { get; set; }
        public List<Accommodation>? accommodations { get; set; }
        public List<FareOffer>? fareOffers { get; set; }
        public List<Service>? services { get; set; }
        public List<ServiceOffer>? serviceOffers { get; set; }
        public EditTravelersEligibility? editTravelersEligibility { get; set; }
        public List<ProductAssignment>? productAssignments { get; set; }
        public int numberOfTravelDays { get; set; }
        public string? validityDuration { get; set; }
        public string? activationDuration { get; set; }
        public ActivationPeriod? activationPeriod { get; set; }
        public ValidityPeriod? validityPeriod { get; set; }
        public TravelClass? travelClass { get; set; }
        public List<Condition>? conditions { get; set; }
        public List<string>? tags { get; set; }
        public string? label { get; set; }
        public List<MobileReference>? mobileReferences { get; set; }
        public string? paymentId { get; set; }
        public DateTime firstTravelDate { get; set; }
        public List<Traveler>? travelers { get; set; }
        public bool active { get; set; }
        [NotMapped]
        public double CurrRoe { get; set; }
    }
    public class CabinClass
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
    }
    public class BuyerDetails
    {
        public Agency? agency { get; set; }
    }

    public class CancelationEligibility
    {
        public bool eligible { get; set; }
        public bool partiallyEligible { get; set; }
        public List<CancellableItem>? cancellableItems { get; set; }
    }

    public class CancellableItem
    {
        public List<string>? fareOfferIds { get; set; }
        public OriginalPrices? originalPrices { get; set; }
    }

    public class OriginalPrices
    {
        public List<Billing> billings { get; set; }
        public Selling? selling { get; set; }
        public List<AmountsToDisplay> amountsToDisplay { get; set; }
    }

    public class ComfortCategory
    {
        public string? code { get; set; }
        public string? label { get; set; }
    }

    public class Condition
    {
        public string? condition { get; set; }
        public string? type { get; set; }
    }

    public class Contact
    {
        public string? title { get; set; }
        public string? lastName { get; set; }
        public string? emailAddress { get; set; }
    }
    public class Destination
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public string? code { get; set; }
        public string? label { get; set; }
        public string? localLabel { get; set; }
    }
    public class EditTravelersEligibility
    {
        public bool eligible { get; set; }
    }
    public class ExchangeEligibility
    {
        public bool eligible { get; set; }
        public bool routeChangePossible { get; set; }
    }
    public class Email
    {
        public string? id { get; set; }
        public string? status { get; set; }
        public string? from { get; set; }
        public List<string>? to { get; set; }
        public List<Attachment>? attachments { get; set; }
        public string? subject { get; set; }
        public string? emailType { get; set; }
        public string? createdDate { get; set; }
    }
    public class FareP2P1
    {
        public string? label { get; set; }
        public Flexibility? flexibility { get; set; }
        public string? conditions { get; set; }
    }
    public class FareP2P2
    {
        public string? productCode { get; set; }
        public List<string>? segments { get; set; }
        public List<string>? travelers { get; set; }
    }

    public class Fare : FareP2P1
    {
        public int numberOfTravelDays { get; set; }
        public string? validityDuration { get; set; }
        public string? activationDuration { get; set; }
    }

    public class Fees
    {
        public List<Item>? items { get; set; }
    }

    public class InvoicedPrices
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
        public Fees? fees { get; set; }
        public Total? total { get; set; }
    }

    public class Item
    {
        public string? label { get; set; }
        public Price? price { get; set; }
        public Products? product { get; set; }
        public string? paymentId { get; set; }
    }

    public class KeyAccount
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
    }
    public class LegResponse
    {
        public string? id { get; set; }
        public Origin? origin { get; set; }
        public Destination? destination { get; set; }
        public DateTime? departure { get; set; }
        public DateTime? arrival { get; set; }
        public string? duration { get; set; }
        public ExchangeEligibility? exchangeEligibility { get; set; }
        public List<Segment>? segments { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
        public Prices? prices { get; set; }
        public bool canceled { get; set; }
        public string? paymentId { get; set; }
    }

    public class MobileReference
    {
        public string? reference { get; set; }
        public string? travelerId { get; set; }
        public string? status { get; set; }
    }

    public class NetAmount
    {
        public double value { get; set; }
        public string? currency { get; set; }
    }

    public class PartnerCommission
    {
        public Amount? amount { get; set; }
    }

    public class Payments
    {
        public string? id { get; set; }
        public string? merchantReference { get; set; }
        public string? paymentStatus { get; set; }
        public Amount? amount { get; set; }
        public PaymentMethod? paymentMethod { get; set; }
        public List<PaymentTransaction>? paymentTransactions { get; set; }
        public bool allowancePayment { get; set; }
        public bool paymentDone { get; set; }
        public bool creditCardPayment { get; set; }
    }
    public class Payment
    {
        public List<string>? authorizedPaymentMethods { get; set; }
    }

    public class PaymentMethod
    {
        public string? type { get; set; }
    }

    public class PaymentTransaction
    {
        public string? transactionType { get; set; }
        public Amount? amount { get; set; }
        public string? transactionId { get; set; }
        public string? transactionStatus { get; set; }
        public string? transactionDate { get; set; }
        public string? relatedTransaction { get; set; }
    }

    public class PointOfSale
    {
        public string? code { get; set; }
        public Profile? profile { get; set; }
        public string? entity { get; set; }
    }

    public class Price
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
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

    public class Products
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
        public List<string>? places { get; set; }
        public bool? externalReservation { get; set; }
    }

    public class Product
    {
        public string? code { get; set; }
        public string? label { get; set; }
        public string? family { get; set; }
        public List<string>? categories { get; set; }
        public Applicability? applicability { get; set; }
        public string? method { get; set; }
    }

    public class ProductAssignment
    {
        public string? id { get; set; }
        public string? travelerId { get; set; }
        public Prices? prices { get; set; }
        public string? productCode { get; set; }
        public CancelationEligibility? cancelationEligibility { get; set; }
        public bool? canceled { get; set; }
    }

    public class Profile
    {
        public string? marketType { get; set; }
        public string? salesChannel { get; set; }
        public string? zone { get; set; }
        public string? country { get; set; }
        public string? city { get; set; }
        public AuthorizedProducts? authorizedProducts { get; set; }
        public Payments? payment { get; set; }
        public Agency? agency { get; set; }
        public KeyAccount? keyAccount { get; set; }
        public string? timezone { get; set; }
        public string? defaultPriceToDisplay { get; set; }
        public bool groupBookingAllowed { get; set; }
        public string? billingMethod { get; set; }
    }

    public class CreateBookingResponse
    {
        public string? id { get; set; }
        public string? correlationid { get; set; }
        public string? bookingReference { get; set; }
        public string? bookingStatus { get; set; }
        public List<BookingItem>? bookingItems { get; set; }
        public List<Billing>? billings { get; set; }
        public List<Payment>? payments { get; set; }
        public Contact? contact { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastStatusModificationTime { get; set; }
        public string? lastStatusModifiedBy { get; set; }
        public PointOfSale? pointOfSale { get; set; }
        public Prices? prices { get; set; }
        public List<Email>? emails { get; set; }
        public List<StatusChange>? statusChanges { get; set; }
        public DateTime? expirationDate { get; set; }
        public DateTime? holdDistributorTTLDate { get; set; }
        public InvoicedPrices? invoicedPrices { get; set; }
        public string? overviewStatus { get; set; }
        public bool billingDocumentsEligibility { get; set; }
    }

    public class Selling
    {
        public SellingPrice? sellingPrice { get; set; }
        public PartnerCommission? partnerCommission { get; set; }
        public SellingPrice? agentSellingPrice { get; set; }
        public PartnerCommission? agentCommission { get; set; }
    }

    public class SellingPrice
    {
        public Amount? amount { get; set; }
    }

    public class StatusChange
    {
        public string? pointOfSale { get; set; }
        public string? modifiedByClientId { get; set; }
        public string? status { get; set; }
        public string? date { get; set; }
    }

    public class SupplierOrder
    {
        public string? orderStatus { get; set; }
        public List<SupplierReference>? supplierReferences { get; set; }
        public bool voidable { get; set; }
    }

    public class SupplierReference
    {
        public string? pnr { get; set; }
        public string? supplierReferenceStatus { get; set; }
    }

    public class Ticket
    {
        public string? productAssignment { get; set; }
        public string? ticketNumber { get; set; }
        public bool fictive { get; set; }
    }

    public class TicketingOption
    {
        public string? code { get; set; }
    }

    public class TicketingOptionSelected
    {
        public string? code { get; set; }
    }

    public class Total
    {
        public List<BillingsGross>? billingsGross { get; set; }
        public List<BillingsNet>? billingsNet { get; set; }
    }

    public class TravelClass
    {
        public string? code { get; set; }
        public ComfortCategory? comfortCategory { get; set; }
    }

    public class Traveler
    {
        public string? id { get; set; }
        public string? type { get; set; }
        public int age { get; set; }
        public string? title { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public bool leadTraveler { get; set; }
        public string? dateOfBirth { get; set; }
        public string? emailAddress { get; set; }
        public string? phoneNumber { get; set; }
        public string? countryOfResidence { get; set; }
        public TravelerDocument? travelerDocument { get; set; }
        public bool canceled { get; set; }
        public List<Cards>? cards { get; set; }
    }

    public class TravelerInformationRequired
    {
        public List<string>? defaultTravelerInformationRequired { get; set; }
        public List<string>? leadTravelerInformationRequired { get; set; }
    }
    public class ValueDocument
    {
        public string? id { get; set; }
        public string? url { get; set; }
        public int size { get; set; }
    }
    public class Reservation
    {
        public List<Billing>? billings { get; set; }
        public Selling? selling { get; set; }
    }
}
