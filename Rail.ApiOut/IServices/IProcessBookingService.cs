using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.Bookings;
using Rail.BO.Entities;
using Rail.BO.P2PModel;
using Rail.BO.ViewModel;

namespace Rail.ApiOut.IServices
{
    public interface IProcessBookingService
    {
        Task<CurrentAgent> InjectAgentDetails(string agentId);
        Task<string> ReturnSelfBalanceCreditLimit(PaymentDetailsViewModel paymentDetailsModel, CurrentAgent objCurrentAgent, decimal total_price);
        Task<List<long>> GetBookingitemId(long bookingId);
        Task<BookingModel> GetBookings(string bookingId, string correlationId);
        Task<List<BookingItemsModel>> GetBookingitems(long bookingId);
        Task<decimal> CalculateTotalAmountToPay(List<decimal> totalAgentAmount, decimal BookingFee, decimal MarkUpBookingFees, decimal currROEEUR, decimal ReservationFeeEur);
        Task<BookingModel> GetBookingByBookingId(string bookingId);
        Task<long> GetBookingItemIdByOffer(string offer);
        Task<long> GetPaxIdByOffer(long fk_itemId);
        Task<BookingItemsModel> CreateBookingItemPass(SearchHistoryModel searchresult, BO.Offer offers, string conditionJson, string pricingJson, string travelersJSon, string item, string _correlation, string _Currency, string _AgentID);
        Task<BookingItemsModel> CreateBookingItemP2P(TicketsModelResponse res, BO.P2PModel.Offer offers, Solution solution, string travelersJSon, string item, string _correlation, string _Currency, string _AgentID);

        Task<(List<BookingItemsModel>, string, List<KeyValuePair<string, string>>, List<decimal>)> HandlePassCondition(
          SearchHistoryModel searchresult, string item, string _correlation, string _Currency, string _AgentID, string baseUrl);

        Task<(List<BookingItemsModel> ticketItems, string result, List<KeyValuePair<string, string>>, List<decimal> totalAgentAmount, List<string> location)>
            HandleElseCondition(SearchHistoryModel searchResult, List<string> offerLocations, string item, List<string> location, string _correlation, string _Currency, string _AgentID);
        Task<List<BookingSummaryModel>> GetBookingSummaryByDateAsync(DateTime startDate, DateTime endDate, string _AgentID);

        Task<bool> CheckExistCorrelation(string correlationId);
    }
}
