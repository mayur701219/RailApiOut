using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rail.ApiOut.IServices;
using Rail.ApiOut.Services;
using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.ViewModel;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Rail.ApiOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : BaseRailController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly IProcessBookingService _processBooking;


        private string baseUrl = string.Empty;


        public RefundController(IHttpContextAccessor httpContextAccessor, IHelper helper, IConfiguration configuration, IProcessBookingService processBooking) : base(httpContextAccessor, helper)
        {
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
            baseUrl = configuration.GetSection("Links").GetSection("baseUrl").Value;
            _processBooking = processBooking;
        }

        [HttpGet]
        [Route("GetRefundItems")]
        public async Task<IActionResult> GetRefundItems(string bookingId)
        {
            string result = string.Empty;
            DetailsViewModel? response = new DetailsViewModel();
            List<string> bookingItems = new List<string>();
            try
            {
                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/GetDetailsApiOut?bookingId=" + bookingId, "", HttpMethod.Get);
                response = JsonConvert.DeserializeObject<DetailsViewModel>(result);
                var details = JsonConvert.DeserializeObject<Rail.BO.Bookings.CreateBookingResponse>(response.bookingModel.Response);
                foreach (var item in response.bookingItems.Where(x => x.ParentId == 0))
                {
                    var cancelationEligibility = details.bookingItems.First(x => x.id == item.bookingItemId).cancelationEligibility;
                    bool isEligible = cancelationEligibility.eligible;

                    if (isEligible == true)
                    {
                        bookingItems.Add(item.bookingItemId);
                    }
                }

                var data = new
                {
                    bookingId,
                    bookingItems
                };

                var apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = data,
                    Errors = null
                };

                return new JsonResult(new { apiResponse });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("GetRefundQuote")]
        public async Task<IActionResult> GetRefundQuote(RefundQuoteReqeust refundQuote)
        {
            string result = string.Empty;
            DetailsViewModel? response = new DetailsViewModel();
            try
            {
                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/GetDetailsApiOut?bookingId=" + refundQuote.bookingId, "", HttpMethod.Get);
                response = JsonConvert.DeserializeObject<DetailsViewModel>(result);
                var details = JsonConvert.DeserializeObject<Rail.BO.Bookings.CreateBookingResponse>(response.bookingModel.Response);
                RefundQuoteRequestVM quoteRequest = new RefundQuoteRequestVM();

                foreach (var item in refundQuote.bookingitemId)
                {
                    var bookingitem = details.bookingItems.First(x => x.id == item);
                    var cancelationEligibility = details.bookingItems.First(x => x.id == item).cancelationEligibility.cancellableItems;
                    if (bookingitem.type == "point-to-point")
                    {
                        if (cancelationEligibility != null)
                        {
                            foreach (var eligible in cancelationEligibility)
                            {
                                ItemRefund itemRefund = new ItemRefund();
                                foreach (var fare in eligible.fareOfferIds)
                                {
                                    CancellableItem item1 = new CancellableItem();
                                    item1.id = fare;
                                    itemRefund.cancellableItems.Add(item1);
                                }
                                itemRefund.id = bookingitem.id;
                                quoteRequest.items.Add(itemRefund);
                            }
                        }
                        else
                        {
                            foreach (var fares in bookingitem.fareOffers)
                            {
                                ItemRefund itemRefund = new ItemRefund();
                                CancellableItem item1 = new CancellableItem();
                                item1.id = fares.id;                                
                                itemRefund.id = bookingitem.id;
                                quoteRequest.items.Add(itemRefund);
                            }
                        }

                    }
                    else
                    {
                        ItemRefund itemRefund = new ItemRefund();
                       
                        itemRefund.id = bookingitem.id;
                        quoteRequest.items.Add(itemRefund);
                    }
                    quoteRequest.bookingId = refundQuote.bookingId;
                    quoteRequest.pk_bookingId = response.bookingModel.Id;
                    quoteRequest.type = "REFUND";
                }

                result = await _helper.ExecuteAPI(baseUrl + "/api/Refund/RefundQuote", JsonConvert.SerializeObject(quoteRequest), HttpMethod.Post);

                if (result.Contains("SUCCESS"))
                {
                    result = result.Substring(8);
                    var parsedResp = JsonConvert.DeserializeObject<RefundQuoteResponse>(result);
                    decimal totalRefund = 0;


                    var itemArray = quoteRequest.items.Select(x => x.id).ToList();
                    response.bookingItems = response.bookingItems.Where(x => itemArray.Contains(x.bookingItemId)).ToList();

                    RefundResponseApi refundResponseApi = new RefundResponseApi();
                    List<RefundReponse> refundlst = new List<RefundReponse>();
                    refundResponseApi.RefundId = parsedResp.id;
                    #region Pass Calculation
                    var passes = response.bookingItems.Where(x => x.Type == "PASS").ToList();
                    if (passes.Count > 0)
                    {
                        foreach (var pass in passes)
                        {
                            RefundReponse refund = new RefundReponse();
                            var quoteitem = parsedResp.items.Where(x => x.id == pass.bookingItemId).FirstOrDefault();
                            double currROE = pass.FinalROE;
                            var paxdetails = response.paxDetails.Where(x => x.fk_ItemId == pass.Id);
                            var adult = paxdetails.Count(x => x.type == "ADULT");
                            var senior = paxdetails.Count(x => x.type == "SENIOR");
                            var youth = paxdetails.Count(x => x.type == "YOUTH");
                            var agentsellingprice = quoteitem.reversedPrices.selling.agentSellingPrice.amount.value;
                            var displayamount = Math.Round(agentsellingprice * Convert.ToDecimal(currROE), 2);
                            totalRefund += displayamount;


                            refund.Type = "Pass";
                            refund.Source = pass.Title;
                            refund.Pax = adult + " Adults" + "," + youth + " Youth" + " & " + senior + " Senior";
                            refund.Pnr = pass.PNR;
                            refund.OriginalPrice = pass.Currency + " " + pass.AgentAmount + " / " + pass.AgentCurrency + " " + Math.Round(pass.AgentAmount * Convert.ToDecimal(currROE), 2);
                            refund.RefundAmount = pass.Currency + " " + quoteitem.reversedPrices.selling.agentSellingPrice.amount.value + " / " + pass.AgentCurrency + " " + displayamount;
                            refundlst.Add(refund);
                        }
                    }
                    #endregion

                    #region Ticket Calculation
                    var tickets = response.bookingItems.Where(x => x.Type == "Ticket" && x.ParentId == 0).ToList();
    
                    if (tickets.Count > 0)
                    {
                        foreach (var ticket in tickets)
                        {
                            RefundReponse refund = new RefundReponse();
                            var quoteitem = parsedResp.items.Where(x => x.id == ticket.bookingItemId).FirstOrDefault();
                            double currROE = ticket.FinalROE;
                            var paxdetails = response.paxDetails.Where(x => x.fk_ItemId == ticket.Id);
                            var adult = paxdetails.Count(x => x.type == "ADULT");
                            var senior = paxdetails.Count(x => x.type == "SENIOR");
                            var youth = paxdetails.Count(x => x.type == "YOUTH");
                            var agentsellingprice = quoteitem.reversedPrices.selling.agentSellingPrice.amount.value;
                            var displayamount = Math.Round(agentsellingprice * Convert.ToDecimal(currROE), 2);
                            totalRefund += displayamount;

                            refund.Type = "Origin & Destination";
                            refund.Source = ticket.Origin + " To " + ticket.Destination;
                            refund.Pax = adult + " Adults" + "," + youth + " Youth" + " & " + senior + " Senior";
                            refund.Pnr = ticket.PNR;
                            refund.OriginalPrice = ticket.Currency + " " + ticket.AgentAmount + " / " + ticket.AgentCurrency + " " + Math.Round(ticket.AgentAmount * Convert.ToDecimal(currROE), 2);
                            refund.RefundAmount = ticket.Currency + " " + quoteitem.reversedPrices.selling.agentSellingPrice.amount.value + " / " + ticket.AgentCurrency + " " + displayamount;
                            refundlst.Add(refund);
                        }
                    }

                    #endregion

                    refundResponseApi.refundReponse = refundlst;
                    refundResponseApi.Totalamount = totalRefund;


                    var apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = refundResponseApi,
                        Errors = null
                    };
                    return new JsonResult(new { apiResponse });

                }
                else
                {
                    var apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = result,
                        Errors = null
                    };

                    return new JsonResult(new { apiResponse });

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("ConfirmRefund")]
        public async Task<IActionResult> ConfirmRefund(ConfirmRefundRequest request)
        {
            string result = string.Empty;
            string returnResult = string.Empty;
            var booking = await _processBooking.GetBookingByBookingId(request.bookingId);
            try
            {
                var response = await _helper.ExecuteAPI(baseUrl + "/api/Refund/ConfirmRefund?bookingId=" + request.bookingId + "&refundOperationId=" + request.refundId, "", HttpMethod.Post);
                if (response.Contains("SUCCESS"))
                {
                    response = response.Substring(8);
                    string _data = await _helper.ExecuteAPI(baseUrl + "/api/Booking/GetDetails?id=" + booking.Id, "", HttpMethod.Get);
                    var details = JsonConvert.DeserializeObject<DetailsViewModel>(_data);
                    decimal totalRefund = 0;

                    var parsedResp = JsonConvert.DeserializeObject<RefundQuoteResponse>(response);

                    foreach (var item in parsedResp.items)
                    {
                        var agentsellingprice = item.reversedPrices.selling.sellingPrice.amount.value;
                        var bookingitem = details.bookingItems.Where(x=>x.bookingItemId == item.id).FirstOrDefault();
                        var displayamount = Math.Round(agentsellingprice * Convert.ToDecimal(bookingitem.FinalROE), 2);
                        totalRefund += displayamount;
                    }

                    double AmountToRefund = Math.Round(Convert.ToDouble(totalRefund), 0);
                    Dictionary<string, string> paramters = new Dictionary<string, string>();
                    var _bookingmodel = details.bookingModel;

                    if (_bookingmodel.PaymentMode == "AgentCreditBalance")
                    {
                        paramters.Add("UserId", _bookingmodel.AgentId.ToString());
                        paramters.Add("AgentNo", _bookingmodel.AgentId.ToString());
                        paramters.Add("Balance", Convert.ToString(AmountToRefund));
                        paramters.Add("TransactionType", "Credit");
                        paramters.Add("OrderId", _bookingmodel.BookingId);

                        result = await _helper.ExecuteAPI(baseUrl + "/api/TrvlNxtConsole/UpdateAgentBalance", JsonConvert.SerializeObject(paramters), HttpMethod.Post);
                        returnResult = "Refund was successful";
                    }

                  
                    var apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = returnResult,
                        Errors = null
                    };
                    return new JsonResult(new { apiResponse });
                }
                else
                {
                    var apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = "Something went wrong",
                        Errors = null
                    };
                    return new JsonResult(new { apiResponse });

                }
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Something went wrong",
                    Errors = null
                };
                return new JsonResult(new { apiResponse });
            }

        }
    }
}
