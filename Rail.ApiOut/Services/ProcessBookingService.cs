using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.Entities;
using Rail.BO.P2PModel;
using Rail.BO.ViewModel;

namespace Rail.ApiOut.Services
{
    public class ProcessBookingService : IProcessBookingService
    {
        private readonly IHelper _helper;
        private string baseUrl = string.Empty;
        private readonly IConfiguration _configuration;
        private readonly RailDBContext _db;

        public ProcessBookingService(IHelper helper, IConfiguration configuration, RailDBContext db)
        {
            _helper = helper;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("Links").GetSection("baseUrl").Value;
            _db = db;
        }

        public async Task<CurrentAgent> InjectAgentDetails(string agentId)
        {
            string MainAgentId, queryString, result, AgencyId = string.Empty;
            AgencyDetails agencyDetails = new AgencyDetails();
            List<ServiceFee> objServiceFee = new List<ServiceFee>();
            CurrentAgent objCurrAgent = new CurrentAgent();

            try
            {
                //if (!string.IsNullOrEmpty(AgencyId))
                {
                    //CookieID = CookieID != null ? CookieID : SessionID;
                    //MainAgentId = await _sessionHelper.GetSessionRail("MainAgentId", CookieID);
                    //AgencyId = await _sessionHelper.GetSessionRail("AgentID", CookieID);
                    //queryString = "MainAgentId=" + MainAgentId + "&AgencyId=" + AgencyId;
                    //_logService.Log(new ErrorLogsModel() { Request = queryString, URL = "/api/TrvlNxtConsole/FetchAllAgencies" });
                    queryString = "MainAgentId=0" + "" + "&AgencyId=" + agentId;
                    result = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/GetAgencyDetails?" + queryString, "", HttpMethod.Get);
                    agencyDetails = AgencyDetails.FromJson(result); //JsonConvert.DeserializeObject<AgencyDetails>(result);
                    string AllowBooking = string.Empty;
                    if (string.IsNullOrEmpty(agentId) || agentId == "0")
                    {
                        var forUser = (agencyDetails.Agency.MyProperty.AllowAgentBooking != null) ? agencyDetails.Agency.MyProperty.AllowAgentBooking.FirstOrDefault(x => x.Product == "Rail") : null;
                        AllowBooking = (forUser != null) ? forUser.AllowBooking : "false";
                    }
                    else
                    {
                        var forAgent = (agencyDetails.Agency.AllowRiyaUserBooking != null) ? agencyDetails.Agency.AllowRiyaUserBooking.FirstOrDefault(x => x.Product == "Rail") : null;
                        AllowBooking = (forAgent != null) ? forAgent.AllowBooking : "false";
                    }
                    objCurrAgent = new CurrentAgent
                    {
                        RiyaUserId = "0",
                        AgencyId = agentId,
                        Text = agencyDetails.Agency.MyProperty.AgencyName,
                        CreditLimit = (agencyDetails.Agency.MyProperty.AgentBalance ?? 0).ToString(),
                        SelfBalance = (agencyDetails.Agency.UserBalance ?? 0).ToString(),
                        Currency = agencyDetails.Agency.MyProperty.BaseCurrency,
                        Country = agencyDetails.Agency.MyProperty.BookingCountry,
                        AgentMobile = agencyDetails.Agency.MyProperty.AgentMobileNo,
                        AgentEmailId = agencyDetails.Agency.MyProperty.AgentEmailID,
                        RiyaUserMobile = agencyDetails.Agency.UserMobileNo,
                        RiyaEmailID = agencyDetails.Agency.UserEmailID,
                        MarkUpBookingFees = 0,
                        AllowBooking = AllowBooking,
                    };
                    //objServiceFee = await FetchServicFee(AgencyId);
                    string correlationquery = "?AgentId=" + objCurrAgent.AgencyId + "&RiyaUserId=" + objCurrAgent.RiyaUserId;
                    agencyDetails.CorrelationId = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/GetCorrelationId" + correlationquery, "", HttpMethod.Get);
                    //_httpContextAccessor.HttpContext.Session.SetObjectAsJson("SelectedAgency", objCurrAgent);
                    //await _sessionHelper.SetAgentSession(objCurrAgent, CookieID);

                    //_httpContextAccessor.HttpContext.Session.SetObjectAsJson("AgencyServiceFee", (objServiceFee.Count > 0) ? objServiceFee : null);
                }
            }
            catch (Exception) { throw; }
            return objCurrAgent;
        }

        public async Task<string> ReturnSelfBalanceCreditLimit(PaymentDetailsViewModel paymentDetailsModel, CurrentAgent objCurrentAgent, decimal total_price)
        {
            string result = string.Empty;
            AgentBalanceDetails agentBalance = new AgentBalanceDetails();
            try
            {
                Dictionary<string, string> paramters = new Dictionary<string, string>();
                if (paymentDetailsModel.radio_pay_method == "AgentCreditBalance")
                {
                    if (!(Convert.ToDecimal(paymentDetailsModel.AmountPaidByAgent) > Convert.ToDecimal(objCurrentAgent.CreditLimit)))
                    {
                        paramters.Add("UserId", objCurrentAgent.AgencyId);
                        paramters.Add("AgentNo", objCurrentAgent.AgencyId);
                        paramters.Add("Balance", Convert.ToString(total_price));
                        paramters.Add("TransactionType", "Credit");
                        paramters.Add("OrderId", paymentDetailsModel.hdnbookingId);

                        result = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/UpdateAgentBalance", JsonConvert.SerializeObject(paramters), HttpMethod.Post);
                    }
                }
                else
                {
                    if (!(Convert.ToDecimal(paymentDetailsModel.AmountPaidByAgent) > Convert.ToDecimal(objCurrentAgent.SelfBalance)))
                    {
                        paramters.Add("UserId", objCurrentAgent.RiyaUserId);
                        paramters.Add("CreatedBy", objCurrentAgent.RiyaUserId);
                        paramters.Add("Balance", Convert.ToString(total_price));
                        paramters.Add("Country", objCurrentAgent.Country);
                        paramters.Add("TransactionType", "Credit");
                        paramters.Add("OrderId", paymentDetailsModel.hdnbookingId);//    Guid.NewGuid().ToString());

                        result = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/UpdateRiyaAgentBalance", JsonConvert.SerializeObject(paramters), HttpMethod.Post);
                    }
                }
                #region Update RiyaAgent or Agent Balance API

                agentBalance = AgentBalanceDetails.FromJson(result);
                if ((bool)agentBalance.IsSuccess)
                {
                    if (paymentDetailsModel.radio_pay_method == "AgentCreditBalance")
                    {
                        objCurrentAgent.CreditLimit = agentBalance.Agency.Balance;
                    }
                    else
                    {
                        objCurrentAgent.SelfBalance = agentBalance.Agency.Balance;
                    }

                    //_httpContextAccessor.HttpContext.Session.SetObjectAsJson("SelectedAgency", objCurrentAgent);
                    //await _sessionHelper.SetAgentSession(objCurrentAgent, CookieID);
                }
                #endregion

                return result;
            }
            catch (Exception) { throw; }
        }

        public async Task<List<long>> GetBookingitemId(long bookingId)
        {
            return await _db.cartModel.Where(x => x.fk_bookingId == bookingId).Select(x => x.Id).ToListAsync();
        }

        public async Task<BookingModel> GetBookings(string correlationId)
        {
            return await _db.bookings.Where(x => x.CorrelationId == correlationId).FirstOrDefaultAsync();
        }

        public async Task<List<BookingItemsModel>> GetBookingitems(long bookingId)
        {
            return await _db.cartModel.Where(x => x.fk_bookingId == bookingId).ToListAsync();
        }


        public async Task<List<BookingItemsModel>> GetBookingFinalitems(long bookingId)
        {
            return await _db.cartModel.Where(x => x.fk_bookingId == bookingId && x.Status == "CREATED").ToListAsync();
        }

        public async Task<List<PaxDetailModel>> PaxDetails(long fkitemid)
        {
            return await _db.paxDetails.Where(x => x.fk_ItemId == fkitemid).ToListAsync();
        }


        public async Task<decimal> CalculateTotalAmountToPay(List<decimal> totalAgentAmount, decimal BookingFee, decimal MarkUpBookingFees, decimal currROEEUR, decimal ReservationFeeEur)
        {
            decimal gstBookingFee = MarkUpBookingFees * 18 / 100;
            decimal FinalBookingFee = BookingFee + MarkUpBookingFees;

            decimal totalAmountToPay = Math.Ceiling(totalAgentAmount.Sum() + (FinalBookingFee * currROEEUR) + (gstBookingFee * currROEEUR) + (ReservationFeeEur * currROEEUR));

            return totalAmountToPay;
        }


        public async Task<BookingItemsModel> CreateBookingItemPass(SearchHistoryModel searchresult, BO.Offer offers, string conditionJson, string pricingJson, string travelersJSon, string item, string _correlation, string _Currency, string _AgentID)
        {
            BookingItemsModel bookingItem = new BookingItemsModel();
            bookingItem.Id = 0;
            bookingItem.CorrelationId = _correlation;
            bookingItem.AgentAmount = offers.prices.selling.agentSellingPrice.amount.value;
            bookingItem.AgentCommission = offers.prices.selling.agentCommission.amount.value;
            bookingItem.AgentCurrency = _Currency;
            bookingItem.AgentId = Convert.ToInt64(_AgentID);
            bookingItem.AgentToInrFinal = 0;
            bookingItem.AgentToInrMarkup = null;
            bookingItem.AgentToInrROE = 0;
            bookingItem.Arrival = null;
            bookingItem.Comfort = offers.travelClass.code; //check
            bookingItem.Conditions = conditionJson.ToString();
            bookingItem.Country = " "; //check dropdownvalue
            bookingItem.CurrRoe = 0;
            bookingItem.Currency = offers.prices.selling.agentSellingPrice.amount.currency; //check
            bookingItem.Departure = null;
            bookingItem.Destination = null;
            bookingItem.DeviceId = "NA";
            bookingItem.Duration = null;
            bookingItem.FinalROE = 0;
            bookingItem.Flexibility = null;
            bookingItem.Location = item;
            bookingItem.ModifiedDate = Convert.ToDateTime(offers.activationPeriod.startDate);
            bookingItem.Origin = null;
            bookingItem.PNR = null;
            bookingItem.ParentId = 0;
            bookingItem.PaxDetails = travelersJSon.ToString();
            bookingItem.PaxPricing = pricingJson.ToString();

            bookingItem.ProductName = null;
            bookingItem.ROE = 0;
            bookingItem.ReservationFee = 0;
            bookingItem.Response = null;
            bookingItem.RiyaAmount = offers.prices.selling.sellingPrice.amount.value;
            bookingItem.RiyaCommission = offers.prices.selling.partnerCommission.amount.value;

            bookingItem.RiyaUserId = 0;
            bookingItem.ServiceFeesAppliedId = null;
            bookingItem.Status = "Cart";

            bookingItem.SupplierId = offers.marketingCarrier;
            bookingItem.SupplierToInrFinal = 0;
            bookingItem.SupplierToInrMarkup = null;
            bookingItem.SupplierToInrROE = 0;
            bookingItem.Title = offers.label;
            bookingItem.Type = searchresult.Type;
            bookingItem.activationPeriodEnd = null;

            bookingItem.activationPeriodStart = null;

            bookingItem.bookingId = null;
            bookingItem.bookingItemId = null;
            bookingItem.bookingReference = null;
            bookingItem.expirationDate = offers.expirationDate;
            bookingItem.fk_bookingId = 0;
            bookingItem.isDirect = 0;
            bookingItem.isInBound = false;
            bookingItem.isRoundTrip = false;
            bookingItem.numberOfTravelDays = "15 days";
            bookingItem.roeMarkup = null;
            bookingItem.validityPeriodEnd = null;
            bookingItem.validityPeriodStart = null;

            return bookingItem;
        }


        public async Task<BookingItemsModel> CreateBookingItemP2P(TicketsModelResponse res, BO.P2PModel.Offer offers, Solution solution, string travelersJSon, string item, string _correlation, string _Currency, string _AgentID)
        {
            BookingItemsModel bookingItems = new BookingItemsModel();
            bookingItems.Id = 0;
            bookingItems.fk_bookingId = 0;
            bookingItems.bookingId = null;
            bookingItems.CorrelationId = _correlation;
            bookingItems.Type = "Ticket";
            bookingItems.PNR = null;
            bookingItems.Origin = solution.origin.label;
            bookingItems.Destination = solution.destination.label;
            bookingItems.isDirect = solution.segments.Count();//
            bookingItems.isRoundTrip = Convert.ToBoolean(res.legs[0].directOnly);//
            bookingItems.isInBound = false;//
            bookingItems.Departure = res.legs[0].departure;
            bookingItems.Arrival = solution.arrival;
            bookingItems.Duration = solution.duration;
            bookingItems.Comfort = offers.comfortCategory.label;
            bookingItems.Flexibility = offers.flexibility.label;
            bookingItems.numberOfTravelDays = null;
            bookingItems.expirationDate = null; //offers.expirationDate.ToString();
            bookingItems.activationPeriodStart = null;
            bookingItems.activationPeriodEnd = null;
            bookingItems.validityPeriodStart = null;
            bookingItems.validityPeriodEnd = null;
            bookingItems.Title = null;
            bookingItems.Conditions = null;
            bookingItems.PaxDetails = travelersJSon;
            bookingItems.PaxPricing = JsonConvert.SerializeObject(offers.prices);
            bookingItems.Currency = offers.prices.selling.sellingPrice.amount.currency;
            bookingItems.AgentCurrency = _Currency;
            bookingItems.RiyaCommission = offers.prices.selling.partnerCommission.amount.value;
            bookingItems.RiyaAmount = offers.prices.selling.sellingPrice.amount.value;
            bookingItems.AgentCommission = offers.prices.selling.agentCommission.amount.value;
            bookingItems.AgentAmount = offers.prices.selling.agentSellingPrice.amount.value;
            bookingItems.ReservationFee = 0;
            bookingItems.ROE = 0;
            bookingItems.roeMarkup = null;
            bookingItems.FinalROE = 0;
            bookingItems.SupplierToInrROE = 0;
            bookingItems.SupplierToInrMarkup = null;
            bookingItems.SupplierToInrFinal = 0;
            bookingItems.AgentToInrROE = 0;
            bookingItems.AgentToInrROE = 0;
            bookingItems.AgentToInrMarkup = null;
            bookingItems.AgentToInrFinal = 0;
            bookingItems.Location = offers.location;
            bookingItems.ProductName = offers.ProductName;
            bookingItems.SupplierId = offers.fareOffers[0].fares[0].productCode;
            bookingItems.AgentId = Convert.ToInt64(_AgentID);
            bookingItems.DeviceId = "NA";
            bookingItems.Status = "Cart";
            bookingItems.Response = null;
            bookingItems.ParentId = 0;
            bookingItems.ModifiedDate = DateTime.Now;
            bookingItems.ServiceFeesAppliedId = null;
            bookingItems.CurrRoe = 0;
            return bookingItems;
        }

        public async Task<BookingModel> GetBookingByBookingId(string bookingId)
        {
            return await _db.bookings.Where(x => x.BookingId == bookingId).FirstOrDefaultAsync();

        }

        public async Task<long> GetBookingItemIdByOffer(string offer)
        {
            return await _db.cartModel.Where(x => x.Location == offer).Select(x => x.Id).FirstOrDefaultAsync();
        }


        public async Task<long> GetPaxIdByOffer(long fk_itemId)
        {
            return await _db.paxDetails.Where(x => x.fk_ItemId == fk_itemId).Select(x => x.Id).FirstOrDefaultAsync();            
        }

        public async Task<(List<BookingItemsModel>, string, List<KeyValuePair<string, string>>, List<decimal>)> HandlePassCondition(
        SearchHistoryModel searchresult, string item, string _correlation, string _Currency, string _AgentID, string baseUrl)
        {
            List<BookingItemsModel> items = new List<BookingItemsModel>();
            List<decimal> totalAgentAmount = new List<decimal>();
            List<KeyValuePair<string, string>> travelerIds = new List<KeyValuePair<string, string>>();
            string result = string.Empty;

            PassesModelResponse res = JsonConvert.DeserializeObject<PassesModelResponse>(searchresult.Response);
            var offers = res.offers.FirstOrDefault(x => x.location == item);
            var conditionJson = JsonConvert.SerializeObject(offers.conditions);
            var pricingJson = JsonConvert.SerializeObject(offers.prices);
            var travelersJson = JsonConvert.SerializeObject(res.travelers);

            BookingItemsModel bookingItem = await CreateBookingItemPass(searchresult, offers, conditionJson, pricingJson, travelersJson, item, _correlation, _Currency, _AgentID);

            result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCart", JsonConvert.SerializeObject(bookingItem), HttpMethod.Post);
            if (result.Contains("SUCCESS"))
            {
                string currROEString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={bookingItem.Currency} &To={bookingItem.AgentCurrency}", "", HttpMethod.Get);

                if (decimal.TryParse(currROEString, out decimal currROE))
                {
                    decimal displayAmount = bookingItem.AgentAmount * currROE;
                    displayAmount = Math.Round(displayAmount, 2);
                    totalAgentAmount.Add(displayAmount);
                }
                else
                {
                    throw new Exception("Failed to parse ROE value.");
                }

                items.Add(bookingItem);
                foreach (var ids in res.travelers)
                {
                    travelerIds.Add(new KeyValuePair<string, string>(item, ids.id));
                }
            }

            return (items, result, travelerIds, totalAgentAmount);
        }



        public async Task<(List<BookingItemsModel> ticketItems, string result, List<KeyValuePair<string, string>>, List<decimal> totalAgentAmount)> HandleElseCondition(SearchHistoryModel searchResult, List<string> offerLocations, string item, string _correlation, string _Currency, string _AgentID)
        {
            var result = string.Empty;
            List<KeyValuePair<string, string>> travelerIds = new List<KeyValuePair<string, string>>();
            var totalAgentAmount = new List<decimal>();
            var ticketItems = new List<BookingItemsModel>();

            TicketsModelResponse res = JsonConvert.DeserializeObject<TicketsModelResponse>(searchResult.Response);

            if (res.legs.Count > 1)
            {
                var bookingSectorsList = new List<BookingSectorsModel>();
                var carts = new List<CartClass>();

                var matchingOffers = res.offers
                                        .Where(x => offerLocations.Contains(x.location))
                                        .Select(x => (Offer: x, Location: x.location))
                                        .ToList();

                foreach (var offr in matchingOffers)
                {
                    var solution = res.legs
                                        .SelectMany(leg => leg.solutions)
                                        .FirstOrDefault(solution => solution?.id == offr.Offer.legSolution);

                    var segment = solution.segments;

                    foreach (var seg in segment)
                    {
                        BookingSectorsModel sector = new BookingSectorsModel
                        {
                            Origin = seg.origin.label,
                            Destination = seg.destination.label,
                            Departure = seg.departure,
                            Arrival = seg.arrival,
                            Duration = seg.duration,
                            SequenceNumber = seg.sequenceNumber
                        };
                        bookingSectorsList.Add(sector);
                    }

                    var travelersJson = JsonConvert.SerializeObject(res.travelers);

                    BookingItemsModel bookingItem = await CreateBookingItemP2P(res, offr.Offer, solution, travelersJson, item, _correlation, _Currency, _AgentID);

                    CartClass cartClass = new CartClass
                    {
                        cartModel = bookingItem,
                        SectorList = bookingSectorsList
                    };

                    result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCartP2PRTApiOut", JsonConvert.SerializeObject(carts), HttpMethod.Post);

                    if (result.Contains("SUCCESS"))
                    {
                        long pk_id = Convert.ToInt64(result.Split('|')[1]);
                        ticketItems[0].Id = pk_id;
                        ticketItems[1].ParentId = pk_id;

                        foreach (var ticketItem in ticketItems)
                        {
                            string currROEString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={ticketItem.Currency} &To={_Currency}", "", HttpMethod.Get);

                            if (decimal.TryParse(currROEString, out decimal currROE))
                            {
                                decimal displayAmount = ticketItem.AgentAmount * currROE;
                                displayAmount = Math.Round(displayAmount, 2);
                                totalAgentAmount.Add(displayAmount);
                            }
                            else
                            {
                                throw new Exception("Failed to parse ROE value.");
                            }
                        }

                        ticketItems.AddRange(ticketItems);
                        foreach (var ids in res.travelers)
                        {
                            travelerIds.Add(new KeyValuePair<string, string>(ticketItems[0].Location, ids.id));
                        }
                    }
                    else
                    {
                        throw new Exception("Something went wrong");
                    }
                }
            }
            else
            {
                var offers = res.offers.FirstOrDefault(x => x.location == item);
                var solution = res.legs?
                                   .SelectMany(leg => leg.solutions)
                                   .FirstOrDefault(solution => solution?.id == offers.legSolution);

                var segment = solution?.segments;
                var bookingSectorsList = new List<BookingSectorsModel>();

                foreach (var seg in segment)
                {
                    BookingSectorsModel sector = new BookingSectorsModel
                    {
                        Origin = seg.origin.label,
                        Destination = seg.destination.label,
                        Departure = seg.departure,
                        Arrival = seg.arrival,
                        Duration = seg.duration,
                        SequenceNumber = seg.sequenceNumber
                    };
                    bookingSectorsList.Add(sector);
                }

                var travelersJson = JsonConvert.SerializeObject(res.travelers);

                BookingItemsModel bookingItem = await CreateBookingItemP2P(res, offers, solution, travelersJson, item, _correlation, _Currency, _AgentID);

                foreach (var ids in res.travelers)
                {
                    travelerIds.Add(new KeyValuePair<string, string>(offers.location, ids.id));
                }

                CartClass cartClass = new CartClass
                {
                    cartModel = bookingItem,
                    SectorList = bookingSectorsList
                };

                result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCartP2P", JsonConvert.SerializeObject(cartClass), HttpMethod.Post);

                if (result.Contains("SUCCESS"))
                {
                    string currROEString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={bookingItem.Currency} &To={bookingItem.AgentCurrency}", "", HttpMethod.Get);

                    if (decimal.TryParse(currROEString, out decimal currROE))
                    {
                        decimal displayAmount = bookingItem.AgentAmount * currROE;
                        displayAmount = Math.Round(displayAmount, 2);
                        totalAgentAmount.Add(displayAmount);
                    }
                    else
                    {
                        throw new Exception("Failed to parse ROE value.");
                    }
                    ticketItems.Add(bookingItem);
                }
                else
                {
                    throw new Exception("Something went wrong");
                }
            }

            return (ticketItems, result, travelerIds, totalAgentAmount);
        }


        public async Task<List<BookingSummaryModel>> GetBookingSummaryByDateAsync(DateTime startDate, DateTime endDate, string _AgentID)
        {
            try
            {
                var bookingSummary = await (
                           from booking in _db.bookings
                           where booking.creationDate >= startDate && booking.creationDate <= endDate
                           && booking.AgentId == Convert.ToInt64(_AgentID) 
                           && booking.bookingStatus == "INVOICED"
                           select new BookingSummaryModel
                           {
                               BookingId = booking.BookingId,
                               BookingReference = booking.BookingReference,
                               TotalBillingsGross = booking.totalbillingsGross,
                               Currency = booking.Currency,
                               BookingStatus = booking.bookingStatus,

                               BookingItems = _db.cartModel
                                   .Where(bi => bi.fk_bookingId == booking.Id)
                                   .Select(bi => new BookingItemSummaryModel
                                   {
                                       BookingItemId = bi.bookingItemId,
                                       Origin = bi.Origin,
                                       Destination = bi.Destination,
                                       Departure = bi.Departure,
                                       Arrival = bi.Arrival,
                                       ProductName = bi.ProductName,

                                       PaxDetails = _db.paxDetails
                                           .Where(p => p.fk_ItemId == bi.Id)
                                           .Select(p => new PaxDetailSummaryModel
                                           {
                                               PaxId = p.PaxId,
                                               FirstName = p.firstName,
                                               LastName = p.lastName,
                                               Age = p.age,
                                               EmailAddress = p.emailAddress
                                               //phoneNumber = p.phoneNumber
                                           }).ToList()
                                   }).ToList()
                           }
                       ).ToListAsync();

                return bookingSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
