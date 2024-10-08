using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rail.ApiOut.IServices;
using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.Entities;

namespace Rail.ApiOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PaxController : BaseRailController
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly IProcessBookingService _processBooking;

        private string baseUrl = string.Empty;

        public PaxController(IHttpContextAccessor httpContextAccessor, IHelper helper, IConfiguration configuration, IProcessBookingService processBooking) : base(httpContextAccessor, helper)
        {
            _helper = helper;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("Links").GetSection("baseUrl").Value;
            _processBooking = processBooking;
        }

        [HttpPost]
        [Route("AddPax")]
        public async Task<IActionResult> AddPax(List<PaxDetailApi> paxDetails)
        {
            try
            {
                foreach (var item in paxDetails)
                {
                    item.fk_ItemId = await _processBooking.GetBookingItemIdByOffer(item.location);
                    item.Id = await _processBooking.GetPaxIdByOffer(item.fk_ItemId);
                }

                string jsonRequest = JsonConvert.SerializeObject(paxDetails);
                string result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddPax", jsonRequest, HttpMethod.Post);

                if (!result.Contains("SUCCESS"))
                    return new JsonResult(new ApiResponse { Success = false, Message = "Something Went Wrong" });

                var booking = await _processBooking.GetBookings(paxDetails[0].bookingId, _correlation);
                var userRequest = new UserRequest
                {
                    OptionId = booking.Id.ToString(),
                    AgentId = _AgentID,
                    AgentCurrency = _Currency
                };

                string jsonRequest1 = JsonConvert.SerializeObject(userRequest);
                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/UpdateTravelersApiOut", jsonRequest1, HttpMethod.Post);

                if (result.Contains("SUCCESS"))
                    return new JsonResult(new ApiResponse { Success = true, Message = "Pax details added successfully!" });

                return CreateErrorResponse("Something went wrong");
            }
            catch (Exception)
            {
                return CreateErrorResponse("Something went wrong");
            }
        }

        //[HttpPost]
        //[Route("UpdateTravelers")]
        //public async Task<IActionResult> UpdateTravelers(UserRequestApiOut user)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        UserRequest userRequest = new UserRequest();
        //        userRequest.OptionId = user.bookingId;
        //        userRequest.AgentId = _AgentID;
        //        userRequest.AgentCurrency = _Currency;
        //        string jsonRequest = JsonConvert.SerializeObject(userRequest);
        //        result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/UpdateTravelers", jsonRequest, HttpMethod.Post);
        //        var response = new ApiResponse
        //        {
        //            Success = true,
        //            Message = result,
        //            Errors = null
        //        };
        //        return new JsonResult(new { response });
        //    }
        //    catch (Exception e)
        //    {
        //        return CreateErrorResponse("Something went wrong");
        //    }

        //}

        private IActionResult CreateErrorResponse(string message)
        {
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = null
            };
            return new JsonResult(new { apiResponse });
        }
    }
}
