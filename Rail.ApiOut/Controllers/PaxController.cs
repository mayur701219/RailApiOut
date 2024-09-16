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
    [Authorize]
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
            string result = string.Empty;
            try
            {
                foreach (var item in paxDetails)
                {
                    item.fk_ItemId = await _processBooking.GetBookingItemIdByOffer(item.location);
                    item.Id = await _processBooking.GetPaxIdByOffer(item.fk_ItemId);
                }
                string jsonRequest = JsonConvert.SerializeObject(paxDetails);
                result = await _helper.ExecuteAPI(baseUrl + "/api/Cart/AddPax", jsonRequest, HttpMethod.Post);

                if (result.Contains("SUCCESS"))
                {
                    var booking = await _processBooking.GetBookings(_correlation);
                    UserRequest userRequest = new UserRequest();
                    userRequest.OptionId = booking.Id.ToString();
                    userRequest.AgentId = _AgentID;
                    userRequest.AgentCurrency = _Currency;
                    string jsonRequest1 = JsonConvert.SerializeObject(userRequest);
                    result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/UpdateTravelersApiOut", jsonRequest1, HttpMethod.Post);
                    var response = new ApiResponse
                    {
                        Success = true,
                        Message = "Pax details added successfully!",
                        Errors = null
                    };
                    return new JsonResult(new { response });
                }

                var response1 = new ApiResponse
                {
                    Success = false,
                    Message = "Something Went Wrong",
                    Errors = null
                };
                return new JsonResult(new { response1 });
            }
            catch (Exception e)
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
        [Route("UpdateTravelers")]
        public async Task<IActionResult> UpdateTravelers(UserRequestApiOut user)
        {
            string result = string.Empty;
            try
            {
                UserRequest userRequest = new UserRequest();
                userRequest.OptionId = user.bookingId;
                userRequest.AgentId = _AgentID;
                userRequest.AgentCurrency = _Currency;
                string jsonRequest = JsonConvert.SerializeObject(userRequest);
                result = await _helper.ExecuteAPI(baseUrl + "/api/Booking/UpdateTravelers", jsonRequest, HttpMethod.Post);
                var response = new ApiResponse
                {
                    Success = true,
                    Message = result,
                    Errors = null
                };
                return new JsonResult(new { response });
            }
            catch (Exception e)
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
    }
}
