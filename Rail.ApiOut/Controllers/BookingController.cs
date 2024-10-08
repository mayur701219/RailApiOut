using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rail.ApiOut.IServices;
using Rail.ApiOut.Services;
using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.Bookings;
using Rail.BO.Entities;
using Rail.BO.P2PModel;
using Rail.BO.ViewModel;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace Rail.ApiOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseRailController
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly ISearchService _searchService;
        private string baseUrl = string.Empty;
        private readonly IProcessBookingService _processBooking;

        public BookingController(IHttpContextAccessor httpContextAccessor, IHelper helper, IConfiguration configuration, ISearchService searchService, IProcessBookingService processBooking = null) : base(httpContextAccessor, helper)
        {
            _helper = helper;
            _configuration = configuration;
            _searchService = searchService;
            baseUrl = _configuration.GetSection("Links").GetSection("baseUrl").Value;
            _processBooking = processBooking;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateBooking(CreateApiRequest createApi)
        //{
        //    try
        //    {
        //        await MapToCart(createApi);
        //        await _helper.ExecuteAPI(baseUrl + "/api/Booking/CreateBooking",
        //            JsonConvert.SerializeObject(new UserRequest { RiyaUserId = "0", AgentId = _AgentID, AgentCurrency = "", OptionId = _correlation }),
        //            HttpMethod.Post);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Ok();
        //}

        private async Task MapToCart(CreateApiRequest createApi)
        {
            //List<BookingItemsModel> bookingItemsModel = new List<BookingItemsModel>();
            try
            {
                var searchIds = createApi.request.Select(x => x.SearchId).ToList();
                var history = await _searchService.GetSearch(searchIds);
                if (history.Count == searchIds.Count)
                {
                    foreach (var item in history)
                    {
                        if (item.Type == "PASS")
                        {
                            var responseObj = JsonConvert.DeserializeObject<PassesModelResponse>(item.Response);
                            BookingItemsModel model = MapToPass(responseObj, createApi.request.FirstOrDefault(x => x.SearchId == item.SearchId).offers[0]);
                            //bookingItemsModel.Add(model);
                            await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCart", JsonConvert.SerializeObject(model), HttpMethod.Post);
                        }
                        else if (item.Type == "P2P")
                        {
                            //var responseObj = JsonConvert.DeserializeObject<BO.P2PModel.TicketsModelResponse>(item.Response);
                            //BookingItemsModel model = MapToTicket(responseObj, createApi.request.FirstOrDefault(x => x.SearchId == item.SearchId).offers[0]);
                            //bookingItemsModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            //return bookingItemsModel;
        }

        private BookingItemsModel MapToPass(PassesModelResponse? passesModel, string location)
        {
            BookingItemsModel cartModel = new BookingItemsModel();
            BO.Offer offer = new BO.Offer();
            try
            {
                offer = passesModel.offers.FirstOrDefault(x => x.location == location);
                cartModel.CorrelationId = _correlation;
                cartModel.Type = "Pass";
                cartModel.Country = passesModel.place.label;
                cartModel.isDirect = 0;
                cartModel.isRoundTrip = false;
                cartModel.isInBound = false;
                cartModel.Comfort = offer.travelClass.code;
                var days = (offer.numberOfTravelDays == 0) ? getNumberFromString(offer.validityDuration) : offer.numberOfTravelDays.ToString();
                var daysText = (offer.validityDuration.Contains('M') && (offer.numberOfTravelDays == 0)) ? days + " month" : days + " days";
                cartModel.numberOfTravelDays = daysText;
                cartModel.expirationDate = offer.expirationDate;
                var period = (offer.activationPeriod == null) ? offer.validityPeriod : offer.activationPeriod;
                cartModel.activationPeriodStart = Convert.ToDateTime(period.startDate);
                cartModel.activationPeriodEnd = Convert.ToDateTime(period.endDate);
                cartModel.Title = offer.label;
                cartModel.Conditions = JsonConvert.SerializeObject(offer.conditions);
                cartModel.PaxDetails = JsonConvert.SerializeObject(passesModel.travelers);
                cartModel.PaxPricing = JsonConvert.SerializeObject(offer.travelerPassOffers);
                cartModel.Currency = offer.prices.selling.sellingPrice.amount.currency;
                //cartModel.AgentCurrency = $("#hdnCurrency").val();
                cartModel.AgentCurrency = "INR";
                cartModel.RiyaCommission = Convert.ToDecimal(offer.prices.selling.partnerCommission.amount.value);
                cartModel.RiyaAmount = Convert.ToDecimal(offer.prices.selling.sellingPrice.amount.value);
                cartModel.AgentCommission = Convert.ToDecimal(offer.prices.selling.agentCommission.amount.value);
                cartModel.AgentAmount = Convert.ToDecimal(offer.prices.selling.agentSellingPrice.amount.value);
                cartModel.Location = offer.location;
                cartModel.ProductName = null;
                cartModel.SupplierId = offer.marketingCarrier;
                cartModel.Status = "CART";
                cartModel.AgentId = Convert.ToInt64(_AgentID);
                cartModel.RiyaUserId = 0;
                cartModel.DeviceId = null;
                cartModel.ServiceFeesAppliedId = offer.ServiceFeesAppliedId;
            }
            catch (Exception)
            {
                throw;
            }
            return cartModel;
        }

        private BookingItemsModel MapToTicket(BO.P2PModel.TicketsModelResponse ticketsModel, string location)
        {
            BookingItemsModel cartModel = new BookingItemsModel();
            BO.P2PModel.Offer offer = new BO.P2PModel.Offer();
            try
            {
                //offer = ticketsModel.offers.FirstOrDefault(x => x.location == location);
                //cartModel.CorrelationId = _correlation;
                //cartModel.Type = "Pass";
                //cartModel.Country = passesModel.place.label;
                //cartModel.isDirect = 0;
                //cartModel.isRoundTrip = false;
                //cartModel.isInBound = false;
                //cartModel.Comfort = offer.travelClass.code;
                //var days = (offer.numberOfTravelDays == 0) ? getNumberFromString(offer.validityDuration) : offer.numberOfTravelDays.ToString();
                //var daysText = (offer.validityDuration.Contains('M') && (offer.numberOfTravelDays == 0)) ? days + " month" : days + " days";
                //cartModel.numberOfTravelDays = daysText;
                //cartModel.expirationDate = offer.expirationDate;
                //var period = (offer.activationPeriod == null) ? offer.validityPeriod : offer.activationPeriod;
                //cartModel.activationPeriodStart = Convert.ToDateTime(period.startDate);
                //cartModel.activationPeriodEnd = Convert.ToDateTime(period.endDate);
                //cartModel.Title = offer.label;
                //cartModel.Conditions = JsonConvert.SerializeObject(offer.conditions);
                //cartModel.PaxDetails = JsonConvert.SerializeObject(passesModel.travelers);
                //cartModel.PaxPricing = JsonConvert.SerializeObject(offer.travelerPassOffers);
                //cartModel.Currency = offer.prices.selling.sellingPrice.amount.currency;
                ////cartModel.AgentCurrency = $("#hdnCurrency").val();
                //cartModel.AgentCurrency = "INR";
                //cartModel.RiyaCommission = Convert.ToDecimal(offer.prices.selling.partnerCommission.amount.value);
                //cartModel.RiyaAmount = Convert.ToDecimal(offer.prices.selling.sellingPrice.amount.value);
                //cartModel.AgentCommission = Convert.ToDecimal(offer.prices.selling.agentCommission.amount.value);
                //cartModel.AgentAmount = Convert.ToDecimal(offer.prices.selling.agentSellingPrice.amount.value);
                //cartModel.Location = offer.location;
                //cartModel.ProductName = null;
                //cartModel.SupplierId = offer.marketingCarrier;
                //cartModel.Status = "CART";
                //cartModel.AgentId = Convert.ToInt64(_AgentID);
                //cartModel.RiyaUserId = 0;
                //cartModel.DeviceId = null;
                //cartModel.ServiceFeesAppliedId = offer.ServiceFeesAppliedId;
            }
            catch (Exception)
            {
                throw;
            }
            return cartModel;
        }

        private string getNumberFromString(string data)
        {
            return data.Replace("/\\D/g", "");
        }

        #region CreateBOOking Working 
        [HttpPost]
        [Route("CreateBooking1")]
        public async Task<IActionResult> CreateBooking1(CreateBookingRequestApiOut bookingRequest)
        {
            string result = String.Empty;
            try
            {
                List<BookingItemsModel> items = new List<BookingItemsModel>();
                var searchmodel = await _searchService.GetSearchHistory(_correlation);

                List<decimal> totalAgentAmount = new List<decimal>();

                Dictionary<string, string> travelerIds = new Dictionary<string, string>();
                List<string> locations = new List<string>();
                foreach (var item in bookingRequest.offerLocations)
                {
                    if (locations.Contains(item))
                    {
                        continue;
                    }
                    var searchresult = searchmodel.Where(x => x.Response.Contains(item)).FirstOrDefault();
                    if (searchresult != null)
                    {
                        if (searchresult.Type == "PASS")
                        {
                            PassesModelResponse res = JsonConvert.DeserializeObject<PassesModelResponse>(searchresult.Response);

                            var offers = res.offers.Where(x => x.location == item).FirstOrDefault();

                            var conditionJson = JsonConvert.SerializeObject(offers.conditions);
                            var pricingJson = JsonConvert.SerializeObject(offers.prices);
                            var travelersJSon = JsonConvert.SerializeObject(res.travelers);

                            BookingItemsModel bookingItem = await _processBooking.CreateBookingItemPass(searchresult, offers, conditionJson, pricingJson, travelersJSon, item, _correlation, _Currency, _AgentID);

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
                                    // Handle the case where the API didn't return a valid decimal value
                                    throw new Exception("Failed to parse ROE value.");
                                }
                                items.Add(bookingItem);
                                foreach (var ids in res.travelers)
                                {
                                    travelerIds.Add(item, ids.id);
                                }
                            }
                            else
                            {
                                var apiResponse = new ApiResponse
                                {
                                    Success = false,
                                    Message = "Something Went Wrong",
                                    Errors = null
                                };
                                return new JsonResult(new { apiResponse });
                            }
                        }
                        else
                        {
                            var searchResponse = searchresult.Response;
                            TicketsModelResponse res = JsonConvert.DeserializeObject<TicketsModelResponse>(searchResponse);

                            if (res.legs.Count > 1)
                            {
                                List<BookingSectorsModel> bookingSectorsList = new List<BookingSectorsModel>();
                                List<CartClass> carts = new List<CartClass>();
                                List<BookingItemsModel> bookingitemLst = new List<BookingItemsModel>();


                                var matchingOffers = res.offers
                                                    .Where(x => bookingRequest.offerLocations.Contains(x.location))
                                                    .Select(x => (Offer: x, Location: x.location))
                                                    .ToList();

                                foreach (var offr in matchingOffers)
                                {

                                    var solution = res.legs?
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

                                    var travelersJSon = JsonConvert.SerializeObject(res.travelers);

                                    BookingItemsModel bookingItems = await _processBooking.CreateBookingItemP2P(res, offr.Offer, solution, travelersJSon, item, _correlation, _Currency, _AgentID);

                                    CartClass cartClass = new CartClass();
                                    cartClass.cartModel = bookingItems;
                                    cartClass.SectorList = bookingSectorsList;
                                    carts.Add(cartClass);
                                    locations.Add(offr.Location);
                                    bookingitemLst.Add(bookingItems);
                                }

                                result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCartP2PRTApiOut", JsonConvert.SerializeObject(carts), HttpMethod.Post);

                                if (result.Contains("SUCCESS"))
                                {
                                    long pk_id = Convert.ToInt64(result.Split('|')[1]);
                                    bookingitemLst[0].Id = pk_id;
                                    bookingitemLst[1].ParentId = pk_id;

                                    foreach (var itemlst in bookingitemLst)
                                    {
                                        string currROEString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={itemlst.Currency} &To={_Currency}", "", HttpMethod.Get);

                                        if (decimal.TryParse(currROEString, out decimal currROE))
                                        {
                                            decimal displayAmount = itemlst.AgentAmount * currROE;
                                            displayAmount = Math.Round(displayAmount, 2);
                                            totalAgentAmount.Add(displayAmount);
                                        }
                                        else
                                        {
                                            // Handle the case where the API didn't return a valid decimal value
                                            throw new Exception("Failed to parse ROE value.");
                                        }
                                    }


                                    items.AddRange(bookingitemLst);
                                    foreach (var ids in res.travelers)
                                    {
                                        travelerIds.Add(bookingitemLst[0].Location, ids.id);
                                    }
                                }
                                else
                                {
                                    // Handle the case where the API didn't return a valid decimal value
                                    throw new Exception("Something went wrong");
                                }

                            }
                            else
                            {
                                var offers = res.offers.Where(x => x.location == item).FirstOrDefault();
                                var solution = res.legs?
                                                       .SelectMany(leg => leg.solutions)
                                                       .FirstOrDefault(solution => solution?.id == offers.legSolution);

                                //var solution = res.legs[0].solutions.Where(x => x.id == offers.legSolution).FirstOrDefault();
                                var segment = solution?.segments;
                                List<BookingSectorsModel> bookingSectorsList = new List<BookingSectorsModel>();

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

                                var travelersJSon = JsonConvert.SerializeObject(res.travelers);

                                BookingItemsModel bookingItems = await _processBooking.CreateBookingItemP2P(res, offers, solution, travelersJSon, item, _correlation, _Currency, _AgentID);

                                foreach (var ids in res.travelers)
                                {
                                    travelerIds.Add(offers.location, ids.id);
                                }

                                CartClass cartClass = new CartClass();
                                cartClass.cartModel = bookingItems;
                                cartClass.SectorList = bookingSectorsList;
                                result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddToCartP2P", JsonConvert.SerializeObject(cartClass), HttpMethod.Post);

                                if (result.Contains("SUCCESS"))
                                {
                                    string currROEString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={bookingItems.Currency} &To={bookingItems.AgentCurrency}", "", HttpMethod.Get);

                                    if (decimal.TryParse(currROEString, out decimal currROE))
                                    {
                                        decimal displayAmount = bookingItems.AgentAmount * currROE;
                                        displayAmount = Math.Round(displayAmount, 2);
                                        totalAgentAmount.Add(displayAmount);
                                    }
                                    else
                                    {
                                        // Handle the case where the API didn't return a valid decimal value
                                        throw new Exception("Failed to parse ROE value.");
                                    }
                                    items.Add(bookingItems);
                                }
                                else
                                {
                                    var apiResponse = new ApiResponse
                                    {
                                        Success = false,
                                        Message = "Something Went Wrong",
                                        Errors = null
                                    };
                                    return new JsonResult(new { apiResponse });
                                }
                            }
                        }

                    }
                }

                #region CreateBooking Reqeust

                CreateBookingRequest request = new CreateBookingRequest();
                request.items = new List<BO.Item>();
                request.correlationid = _correlation;
                foreach (var item in items.Where(x => x.ParentId == 0))
                {
                    List<string> liststrings = new List<string>();
                    liststrings.Add(item.Location);
                    if (item.isRoundTrip)
                    {
                        liststrings.Add(items.First(x => x.ParentId == item.Id).Location);
                    }
                    request.items.Add(new BO.Item()
                    {
                        offerLocations = liststrings
                    });
                }

                BookingApiRequest apiRequest = new BookingApiRequest();
                UserRequest user = new UserRequest();
                user.AgentId = _AgentID;
                user.AgentCurrency = _Currency;
                user.RiyaUserId = "0";
                apiRequest.User = user;
                apiRequest.CreateBooking = request;

                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/CreateBokingApiOut", JsonConvert.SerializeObject(apiRequest), HttpMethod.Post);

                #endregion

                BookingResponseApiOut response = JsonConvert.DeserializeObject<BookingResponseApiOut>(result);
                CreateBookingResponse bookingResponse = JsonConvert.DeserializeObject<CreateBookingResponse>(response.EraResponse);

                var booking = await _processBooking.GetBookings(bookingResponse.id,_correlation);

                decimal currROEEUR = 0.00m;
                decimal totalAmountToPay;

                string roeString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={booking.BFCurrency}&To={booking.AgentCurrency}", "", HttpMethod.Get);
                currROEEUR = decimal.Parse(roeString);
                totalAmountToPay = await _processBooking.CalculateTotalAmountToPay(totalAgentAmount, booking.BookingFee, booking.MarkUpOnBookingFee, currROEEUR, 0.00m);

                string bookingID = string.Empty;
                if (response.BookingResponse.Contains("SUCCESS"))
                {
                    bookingID = response.BookingResponse.Substring(8);
                }

                var data = new
                {
                    totalAmountToPay,
                    bookingid = bookingResponse.id,
                    travelerIds
                    //BookBefore =  booking.expirationDate.Value.ToString("dddd, dd MMMM yyyy")
                };
                var apiresponse = new ApiResponse
                {
                    Success = true,
                    Message = JsonConvert.SerializeObject(data),
                    Errors = null
                };

                return new JsonResult(new { apiresponse });
            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Something Went Wrong",
                    Errors = null
                };
                return new JsonResult(new { response });
            }
        }

        #endregion


        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking(CreateBookingRequestApiOut bookingRequest)
        {
            string result = string.Empty;
            try
            {
                var checkExistCorrelation = await _processBooking.CheckExistCorrelation(_correlation);
                if(checkExistCorrelation)
                {
                    var error = new ApiResponse
                    {
                        Success = false,
                        Message = "Correlation Already Exist",
                        Errors = null
                    };
                    return new JsonResult(new { error });
                }

                List<BookingItemsModel> items = new List<BookingItemsModel>();
                var searchmodel = await _searchService.GetSearchHistory(_correlation);

                List<decimal> totalAgentAmount = new List<decimal>();
                List<KeyValuePair<string, string>> travelerIds = new List<KeyValuePair<string, string>>();
                List<string> locations = new List<string>();

                foreach (var item in bookingRequest.offerLocations)
                {
                    if (locations.Contains(item)) continue;

                    var searchresult = searchmodel.Where(x => x.Response.Contains(item)).FirstOrDefault();
                    if (searchresult != null)
                    {
                        if (searchresult.Type == "PASS")
                        {
                            var (passItems, passResult, passTravelerIds, passTotalAgentAmount) = await _processBooking.HandlePassCondition(
                                searchresult, item, _correlation, _Currency, _AgentID, baseUrl);

                            if (!string.IsNullOrEmpty(passResult) && passResult.Contains("SUCCESS"))
                            {
                                items.AddRange(passItems);
                                foreach (var keyValue in passTravelerIds)
                                {
                                    travelerIds.Add(keyValue);
                                }
                                totalAgentAmount.AddRange(passTotalAgentAmount);
                            }
                            else
                            {
                                return new JsonResult(new { apiResponse = new ApiResponse { Success = false, Message = "Something Went Wrong", Errors = null } });
                            }
                        }
                        else
                        {
                            var (ticketItems, ticketResult, ticketTravelerIds, ticketTotalAgentAmount, location) = await _processBooking.HandleElseCondition(
                                searchresult, bookingRequest.offerLocations, item, locations, _correlation, _Currency, _AgentID);

                            if (!string.IsNullOrEmpty(ticketResult) && ticketResult.Contains("SUCCESS"))
                            {
                                items.AddRange(ticketItems);
                                foreach (var keyValue in ticketTravelerIds)
                                {
                                    travelerIds.Add(keyValue);
                                }
                                totalAgentAmount.AddRange(ticketTotalAgentAmount);
                            }
                            else
                            {
                                return new JsonResult(new { apiResponse = new ApiResponse { Success = false, Message = "Something Went Wrong", Errors = null } });
                            }
                        }
                    }
                }

                #region CreateBooking Reqeust

                CreateBookingRequest request = new CreateBookingRequest();
                request.items = new List<BO.Item>();
                request.correlationid = _correlation;
                foreach (var bkitem in items.Where(x => x.ParentId == 0))
                {
                    List<string> liststrings = new List<string>();
                    liststrings.Add(bkitem.Location);
                    if (bkitem.isRoundTrip)
                    {
                        liststrings.Add(items.First(x => x.ParentId == bkitem.Id).Location);
                    }
                    request.items.Add(new BO.Item()
                    {
                        offerLocations = liststrings
                    });
                }

                BookingApiRequest apiRequest = new BookingApiRequest();
                UserRequest user = new UserRequest();
                user.AgentId = _AgentID;
                user.AgentCurrency = _Currency;
                user.RiyaUserId = "0";
                apiRequest.User = user;
                apiRequest.CreateBooking = request;

                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/CreateBokingApiOut", JsonConvert.SerializeObject(apiRequest), HttpMethod.Post);
                if (result.Contains("FAILURE"))
                {

                    BookingResponseApiOut resp = JsonConvert.DeserializeObject<BookingResponseApiOut>(result);
                    var errResponse = JsonConvert.DeserializeObject<ErrorResponse>(resp.EraResponse);
                    var apiresp = new ApiResponse
                    {
                        Success = true,
                        Message = errResponse,
                        Errors = null
                    };

                    return new JsonResult(new { apiresp });
                }
                #endregion

                BookingResponseApiOut response = JsonConvert.DeserializeObject<BookingResponseApiOut>(result);
                CreateBookingResponse bookingResponse = JsonConvert.DeserializeObject<CreateBookingResponse>(response.EraResponse);

                var booking = await _processBooking.GetBookings(bookingResponse.id, _correlation);

                decimal currROEEUR = 0.00m;
                decimal totalAmountToPay;

                string roeString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={booking.BFCurrency}&To={booking.AgentCurrency}", "", HttpMethod.Get);
                currROEEUR = decimal.Parse(roeString);
                totalAmountToPay = await _processBooking.CalculateTotalAmountToPay(totalAgentAmount, booking.BookingFee, booking.MarkUpOnBookingFee, currROEEUR, 0.00m);

                string bookingID = string.Empty;
                if (response.BookingResponse.Contains("SUCCESS"))
                {
                    bookingID = response.BookingResponse.Substring(8);
                }

                var travelerInfoList = bookingResponse.bookingItems.Select(b => new
                {
                    TravelerInformationRequired = new
                    {
                        b.travelerInformationRequired.defaultTravelerInformationRequired,
                        b.travelerInformationRequired.leadTravelerInformationRequired,
                        travelerIds = b.travelers.Select(t => t.id).ToList(),
                        location = b.offerLocations.FirstOrDefault()
                    }
                }).ToList();


                var data = new
                {
                    totalAmountToPay,
                    bookingid = bookingResponse.id,
                    travelerInfoList
                    //BookBefore = booking.expirationDate.Value.ToString("dddd, dd MMMM yyyy")
                };


                var apiresponse = new ApiResponse
                {
                    Success = true,
                    Message = data,
                    Errors = null
                };

                return new JsonResult(new { apiresponse });
            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = null
                };
                return new JsonResult(new { response });
            }
        }

        [HttpPost]
        [Route("PreBooking")]
        public async Task<IActionResult> Prebooking(UserRequestApiOut user)
        {
            string result = String.Empty;
            try
            {
                var booking = await _processBooking.GetBookingByBookingId(user.bookingId);
                if (booking != null)
                {
                    UserRequest userRequest = new UserRequest();
                    userRequest.OptionId = booking.Id.ToString();
                    userRequest.AgentId = _AgentID;
                    userRequest.AgentCurrency = _Currency;
                    string jsonRequest = JsonConvert.SerializeObject(userRequest);
                    result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/Prebooking", jsonRequest, HttpMethod.Post);

                    if (result.Contains("SUCCESS"))
                    {
                        var response = new ApiResponse
                        {
                            Success = true,
                            Message = "Prebooked Successful",
                            Errors = null
                        };
                        return new JsonResult(new { response });
                    }
                    else
                    {
                        var response = new ApiResponse
                        {
                            Success = false,
                            Message = "Something Went Wrong",
                            Errors = null
                        };
                        return new JsonResult(new { response });
                    }

                }
                else
                {
                    var response = new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid booking id",
                        Errors = null
                    };
                    return new JsonResult(new { response });
                }

            }
            catch (Exception)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Something Went Wrong",
                    Errors = null
                };
                return new JsonResult(new { response });
            }
        }

        [HttpPost]
        [Route("ProcessBooking")]
        public async Task<IActionResult> ProcessBooking(PaymentDetailsApiout paymentDetailsModel)
        {
            string IsConfirmed = string.Empty;
            try
            {
                IsConfirmed = await _helper.ExecuteAPI(baseUrl + "/api/Booking/IsConfirmed?bookingId=" + paymentDetailsModel.BookingId, "", HttpMethod.Get);

                if (IsConfirmed == "0")
                {
                    BookingModel booking = await _processBooking.GetBookingByBookingId(paymentDetailsModel.BookingId);
                    List<BookingItemsModel> bookingItems = await _processBooking.GetBookingitems(booking.Id);

                    #region CALCULATIONS
                    List<decimal> totalagentamount = new List<decimal>();
                    foreach (var item in bookingItems)
                    {
                        string currROE = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={item.Currency} &To={item.AgentCurrency}", "", HttpMethod.Get);
                        decimal displayamount = item.AgentAmount * Convert.ToDecimal(currROE);
                        totalagentamount.Add(displayamount);
                    }

                    decimal BookingFee = booking.BookingFee;
                    decimal MarkUpBookingFees = booking.MarkUpOnBookingFee;
                    decimal FinalBookingFee = 0.00m;
                    decimal gstBookingFee = 0.00m;
                    decimal ReservationFeeEur = 0.00m;
                    decimal currROEEUR = 0.00m;
                    decimal totalAmountToPay;

                    string roeString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={booking.BFCurrency}&To={booking.AgentCurrency}", "", HttpMethod.Get);
                    currROEEUR = decimal.Parse(roeString);
                    totalAmountToPay = await _processBooking.CalculateTotalAmountToPay(totalagentamount, BookingFee, MarkUpBookingFees, currROEEUR, ReservationFeeEur);

                    #endregion

                    PaymentDetailsViewModel paymentDetailsView = new PaymentDetailsViewModel();
                    paymentDetailsView.hdnMainId = booking.Id.ToString();
                    paymentDetailsView.hdnbookingId = booking.BookingId;
                    paymentDetailsView.radio_pay_method = "AgentCreditBalance";
                    paymentDetailsView.ModeOfPayment = "0";
                    paymentDetailsView.AmountPaidByAgent = totalAmountToPay.ToString(); // pending 

                    #region ProcessSelfBalanceCreditLimit
                    string result = string.Empty;
                    //bool status = false;
                    decimal total_price = 0;
                    bool isDebited = false;
                    AgentBalanceDetails agentBalance = new AgentBalanceDetails();

                    CurrentAgent objCurrentAgent = await _processBooking.InjectAgentDetails(_AgentID);

                    try
                    {
                        total_price += Convert.ToDecimal(paymentDetailsView.AmountPaidByAgent);

                        Dictionary<string, string> paramters = new Dictionary<string, string>();
                        if (paymentDetailsView.radio_pay_method == "AgentCreditBalance")
                        {
                            //if (!(Convert.ToDecimal(paymentDetailsView.AmountPaidByAgent) > Convert.ToDecimal(objCurrentAgent.CreditLimit)))
                            //{
                            paramters.Add("UserId", objCurrentAgent.AgencyId);
                            paramters.Add("AgentNo", objCurrentAgent.AgencyId);
                            paramters.Add("Balance", Convert.ToString(total_price));
                            paramters.Add("TransactionType", "Debit");
                            paramters.Add("OrderId", paymentDetailsView.hdnbookingId);

                            result = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/UpdateAgentBalance", JsonConvert.SerializeObject(paramters), HttpMethod.Post);
                            //}
                        }
                        else
                        {
                            return new JsonResult(new { message = "Balance is too low for this booking." });
                        }

                        agentBalance = AgentBalanceDetails.FromJson(result);
                        if ((bool)agentBalance.IsSuccess)
                        {
                            isDebited = true;
                            PaymentDetailsModel paymentDetails = new PaymentDetailsModel();
                            paymentDetails.AmountPaidByAgent = paymentDetailsView.AmountPaidByAgent;
                            paymentDetails.MarkUpOnBookingFee = paymentDetailsView.MarkUpBookingFees;
                            paymentDetails.PaymentMode = paymentDetailsView.radio_pay_method;
                            paymentDetails.PaymentType = paymentDetailsView.pgoptionname;
                            paymentDetails.BookingId = paymentDetailsView.hdnbookingId;
                            result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/UpdatePaymentDetails", JsonConvert.SerializeObject(paymentDetails), HttpMethod.Post);

                        }
                    }
                    catch (Exception ex)
                    {
                        if (isDebited)
                        {
                            await _processBooking.ReturnSelfBalanceCreditLimit(paymentDetailsView, objCurrentAgent, total_price);
                        }
                        throw;
                    }
                    #endregion

                    #region  FinalBooking
                    try
                    {

                        result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/Confirmbooking?Id=" + paymentDetailsView.hdnbookingId + "&obtc=" + paymentDetailsView.obtc, "", HttpMethod.Put);
                        if (result.Contains("SUCCESS"))
                        {
                            //await EmailNotification(result.Split("|")[2].ToString(), objCurrentAgent.AgentEmailId, objCurrentAgent.RiyaEmailID, paymentDetailsModel);
                            var response = new ApiResponse
                            {
                                Success = true,
                                Message = "Booking Successful",
                                Errors = null
                            };
                            return new JsonResult(new { response });
                        }
                        else
                        {
                            if (result.Contains("FAILURE"))
                            {
                                if (paymentDetailsView.radio_pay_method != "PaymentGateway")
                                    await _processBooking.ReturnSelfBalanceCreditLimit(paymentDetailsView, objCurrentAgent, total_price);
                                //return RedirectToAction("BookingFailed", new { message = JsonConvert.DeserializeObject<ErrorResponse>(result.Split("|")[1]).label });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (!result.Contains("SUCCESS"))
                        {
                            await _processBooking.ReturnSelfBalanceCreditLimit(paymentDetailsView, objCurrentAgent, total_price);
                        }

                        //ErrorLogsModel errorLogsModel = new ErrorLogsModel
                        //{
                        //    Request = JsonConvert.SerializeObject(paymentDetailsModel),
                        //    Error = e.Message,
                        //    StackTrace = e.StackTrace
                        //};
                        //_logService.Log(errorLogsModel);
                        //return RedirectToAction("BookingFailed", new { message = e.Message });
                    }

                    #endregion
                    var response1 = new ApiResponse
                    {
                        Success = false,
                        Message = "Something Went Wrong",
                        Errors = null
                    };
                    return new JsonResult(new { response1 });
                }
                else
                {
                    return new JsonResult(new { message = "Please wait for some time or check manage booking" });
                }

            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Something Went Wrong",
                    Errors = null
                };
                return new JsonResult(new { response });
            }
        }


        [HttpPost]
        [Route("HoldBooking")]
        public async Task<IActionResult> HoldBooking(PaymentDetailsApiout paymentDetailsModel)
        {
            string result = string.Empty;
            try
            {
                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/Holdbooking?Id=" + paymentDetailsModel.BookingId, "", HttpMethod.Put);
                if (result.Contains("SUCCESS"))
                {
                    var response1 = new ApiResponse
                    {
                        Success = true,
                        Message = "Booking Hold Successfully",
                        Errors = null
                    };
                    return new JsonResult(new { response1 });
                }

                var response = new ApiResponse
                {
                    Success = true,
                    Message = result,
                    Errors = null
                };
                return new JsonResult(new { response });

            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Something Went Wrong",
                    Errors = null
                };
                return new JsonResult(new { response });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooking(string From, string To)
        {
            DateTime fromDate = Convert.ToDateTime(From);
            DateTime toDate = Convert.ToDateTime(To);
            DateTime currentDateTime = DateTime.Now;
            toDate = toDate.Add(currentDateTime.TimeOfDay);

            var bookings = await _processBooking.GetBookingSummaryByDateAsync(fromDate, toDate, _AgentID);
            return new JsonResult(bookings);
        }

        //public async Task<IActionResult> ViewBooking(string bookindId)
        //{
        //    BookingModel booking = await _processBooking.GetBookingByBookingId(bookindId);
        //    List<BookingItemsModel> bookingItems = await _processBooking.GetBookingitems(booking.Id);        

        //}
    }
}
