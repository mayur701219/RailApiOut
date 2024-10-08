using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using Rail.BO;
using Rail.BO.ApiOutModels;
using Rail.BO.CommonFunctions;
using Rail.BO.P2PModel;
using System.Xml.Linq;

namespace Rail.ApiOut.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[ServiceFilter(typeof(TokenAuthorizeFilter))]
    //[Authorize]
    public class SearchController : BaseRailController
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly ISearchService _searchService;
        private string baseUrl = string.Empty;

        public SearchController(IHttpContextAccessor httpContextAccessor, IHelper helper, IConfiguration configuration, ISearchService searchService) : base(httpContextAccessor, helper)
        {
            _helper = helper;
            _configuration = configuration;
            _searchService = searchService;
            baseUrl = _configuration.GetSection("Links").GetSection("baseUrl").Value;
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchRequestModel search)
        {
            string response = string.Empty;
            string modifiedresponse = string.Empty;
           
            string status = "success";
            JObject? _resData = null;
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (search.Type == "PASS")
                    response = await _helper.ExecuteAPI(baseUrl + "/api/Passes/GetPasses?AgentId=" + _AgentID + "&correlation=" + _correlation, JsonConvert.SerializeObject(MapToPass(search)), HttpMethod.Post);
                else if (search.Type == "P2P")
                    response = await _helper.ExecuteAPI(baseUrl + "/api/Tickets/GetTickets?AgentId=" + _AgentID + "&correlation=" + _correlation, JsonConvert.SerializeObject(MapToTicket(search)), HttpMethod.Post);
                else
                    return Content(JsonConvert.SerializeObject(new { status = "error", message = "BadRequest" }), "application/json");

                if (response.Contains("FAILURE") || response.Contains("ERROR"))
                {
                    var _resArr = response.Split("|");
                    status = _resArr[0].ToLower();
                    response = _resArr[1];
                    _resData = JObject.Parse(response);

                    ApiResponse apiresponse = new ApiResponse
                    {
                        Success = false, // Assuming false due to validation error
                        Message = (string)_resData["label"], // Setting Message from the label field
                        Errors = _resData["details"]?.ToObject<List<string>>() // Setting Errors from the details array
                    };
                    return new JsonResult(apiresponse);
                }
                else
                {
                    #region MODIFY RESPONSE

                    bool removeFamilycard = search.isFamilyCard == true && search.From.label == "Switzerland" && search.travelers.Any(x => x.age < 27);
                    modifiedresponse = RemoveUnwantedProperties(response, removeFamilycard);
                    _resData = JObject.Parse(modifiedresponse);
                    string currency = GetFirstBillingCurrency(_resData);

                    string roeString = await _helper.ExecuteAPI($"{baseUrl}/api/Cart/GetROE?From={currency}&To={_Currency}", "", HttpMethod.Get);
                    decimal currROEEUR = decimal.Parse(roeString);

                    AddNewAmountProperties(_resData, _Currency, currROEEUR);
                    UpdateJObjectWithGrossPrice(_resData);
                    string modifiedJsonString = _resData.ToString();

                    #endregion

                    #region Swiss Family Pass

                    //if (search.isFamilyCard == true && search.Type == "PASS" && search.From.label == "Switzerland" && search.travelers.Any(x => x.age < 27))
                    //{
                    //    GetFamilyCardOffers(_resData);
                    //}                    //if (search.isFamilyCard == true && search.Type == "PASS" && search.From.label == "Switzerland" && search.travelers.Any(x => x.age < 27))
                    //{
                    //    GetFamilyCardOffers(_resData);
                    //}
                    #endregion

                    await _searchService.SaveHistory(_resData["id"].ToString(), _correlation, search.Type, response, Convert.ToInt64(_AgentID));
                    _resData.Properties().Where(attr => attr.Name.Contains("pointOfSale")).ToList().ForEach(attr => attr.Remove());
                }

                var _data = JsonConvert.SerializeObject(new { Success = true, data = _resData, Errors = "" });
                return Content(_data, "application/json");

            }
            catch (Exception ex)
            {
                _helper.SendExeptionMail(ex);
                throw;
            }
        }


        public static void GetFamilyCardOffers(JObject rootObject)
        {
            JArray offers = (JArray)rootObject["offers"];

            for (int i = offers.Count - 1; i >= 0; i--)
            {
                JObject offer = (JObject)offers[i];

                // Get the 'tags' array
                JArray tags = (JArray)offer["tags"];

                // Check if 'tags' is not null and contains 'family-card'
                if (tags == null || !tags.Any(tag => tag.ToString() == "family-card"))
                {
                    // Remove the offer if 'family-card' tag is not present
                    offers.RemoveAt(i);
                }
            }

        }

        private static string GetFirstBillingCurrency(JObject rootObject)
        {
            // Navigate to the 'offers' array
            JArray offers = (JArray)rootObject["offers"];
            if (offers == null || offers.Count == 0)
            {
                return null; // No offers available
            }

            // Get the first offer
            string result = offers[0]["prices"]["selling"]["Total"]["amount"]["currency"].ToString();


            return result;
        }


        public static void AddNewAmountProperties(JObject json, string newCurrency, decimal conversionRate)
        {
            // Convert JObject to JArray to iterate through each offer
            JArray offersArray = (JArray)json["offers"];

            foreach (JObject offer in offersArray)
            {
                // Access and update prices for the offer
                JObject prices = (JObject)offer["prices"];
                JObject selling = (JObject)prices["selling"];

                // Update 'agentSellingPrice'
                UpdateAmountProperties(selling, "Total", newCurrency, conversionRate);

                // Update 'partnerCommission'
                UpdateAmountProperties(selling, "Discount", newCurrency, conversionRate);

                // Update 'agentCommission'
                UpdateAmountProperties(selling, "agentCommission", newCurrency, conversionRate);
            }
        }

        private static void UpdateAmountProperties(JObject selling, string key, string newCurrency, decimal conversionRate)
        {
            // Get the amount object
            JObject amount = (JObject)selling[key]["amount"];
            decimal oldValue = (decimal)amount["value"];

            // Calculate new value
            decimal newValue = oldValue * conversionRate;

            newValue = Math.Round(newValue, 2);
            // Add new properties to the amount object
            amount["agentValue"] = newValue;
            amount["agentCurrency"] = newCurrency;
        }


        public static void UpdateJObjectWithGrossPrice(JObject jObject)
        {
            // Check if 'offers' array exists
            var offers = jObject["offers"] as JArray;
            if (offers == null)
                return;

            foreach (var offer in offers)
            {
                var prices = offer["prices"]?["selling"];
                if (prices == null)
                    continue;

                var agentSellingPrice = prices["Total"]?["amount"];
                var partnerCommission = prices["Discount"]?["amount"];

                if (agentSellingPrice == null || partnerCommission == null)
                    continue;

                var agentSellingPriceValue = (decimal?)agentSellingPrice["value"] ?? 0;
                var agentSellingPriceNewValue = (decimal?)agentSellingPrice["agentValue"] ?? 0;
                var partnerCommissionValue = (decimal?)partnerCommission["value"] ?? 0;
                var partnerCommissionNewValue = (decimal?)partnerCommission["agentValue"] ?? 0;

                var grossPriceValue = agentSellingPriceValue + partnerCommissionValue;
                var grossPriceNewValue = agentSellingPriceNewValue + partnerCommissionNewValue;

                grossPriceValue = Math.Round(grossPriceValue, 2);
                string grossCurrency = agentSellingPrice["currency"].ToString();
                grossPriceNewValue = Math.Round(grossPriceNewValue, 2);
                string grossagentCurrency = agentSellingPrice["agentCurrency"].ToString();

                // Create the 'GrossPrice' object
                var grossPrice = new JObject
                {
                    ["value"] = grossPriceValue,
                    ["currency"] = grossCurrency,
                    ["agentValue"] = grossPriceNewValue,
                    ["agentCurrency"] = grossagentCurrency,
                };

                // Add 'GrossPrice' to the 'prices' object
                if (prices["GrossPrice"] == null)
                {
                    prices["Gross"] = grossPrice;
                }
                else
                {
                    prices["Gross"]["value"] = grossPriceValue;
                    prices["Gross"]["currency"] = grossCurrency;
                    prices["Gross"]["agentValue"] = grossPriceNewValue;
                    prices["Gross"]["agentCurrency"] = grossagentCurrency;
                }
            }
        }


        [HttpGet]
        public async Task<string> Countries()
        {
            return await _helper.ExecuteGetAPIFlat(url: baseUrl + "/api/Passes/GetCountries");
        }

        [HttpGet]
        public async Task<string> Places(string query)
        {
            return await _helper.ExecuteAPI(url: baseUrl + "/api/Tickets/GetPlaces?query=" + query + "&correlation=" + _correlation, "", HttpMethod.Get);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private SearchPassesRequest MapToPass(SearchRequestModel search)
        {
            SearchPassesRequest model = new SearchPassesRequest();
            model.validityStartDate = Convert.ToDateTime(search.journeyStartDate).ToString("yyyy-MM-dd");
            model.place = new Place()
            {
                code = search.From.code,
                label = search.From.label,
            };
            model.travelers = search.travelers;
            return model;
        }

        private SearchTicketsRequest MapToTicket(SearchRequestModel search)
        {
            SearchTicketsRequest model = new SearchTicketsRequest();
            var departuredate = Convert.ToDateTime(String.Format("{0} {1}:00", search.journeyStartDate, search.journeyStartTime)).ToString("yyyy-MM-ddTHH:mm:ss");
            model.legs = new List<LegRequest>();
            model.legs.Add(new LegRequest
            {
                departure = Convert.ToDateTime(departuredate),
                destination = new DestinationRequest()
                {
                    code = search.To.code/*String.Format("{0}:{1}", search.To.code, search.To.label)*/,
                    type = search.To.type,
                },
                directOnly = false,
                origin = new OriginRequest()
                {
                    code = search.From.code/*String.Format("{0}:{1}", search.From.code, search.From.label)*/,
                    type = search.From.type,
                }
            });
            if (search.isRoundTrip.Value)
            {
                var returndate = Convert.ToDateTime(String.Format("{0} {1}:00", search.returnDate, search.returnTime)).ToString("yyyy-MM-ddTHH:mm:ss");
                model.legs.Add(new LegRequest
                {
                    departure = Convert.ToDateTime(returndate),
                    destination = new DestinationRequest()
                    {
                        code = search.From.code,
                        type = search.From.type,
                    },
                    directOnly = false,
                    origin = new OriginRequest()
                    {
                        code = search.To.code,
                        type = search.To.type,
                    }
                });
            }
            model.travelers = JsonConvert.DeserializeObject<List<TravelerP2pAge>>(JsonConvert.SerializeObject(search.travelers));
            return model;
        }

        private static void RemoveIds(JObject jObject, string[] props)
        {

            foreach (var offer in jObject["offers"])
            {
                ProcessPrices(offer["prices"] as JObject);

                // Process each travelerPassOffer within the offer
                var travelerPassOffers = offer["travelerPassOffers"] as JArray;
                if (travelerPassOffers != null)
                {
                    foreach (var travelerPassOffer in travelerPassOffers)
                    {
                        ProcessPrices(travelerPassOffer["prices"] as JObject);
                    }
                }
            }
        }

        private static void RemoveFromArray(JArray jArray, string[] props)
        {
            foreach (var jObject in jArray)
            {
                if (jObject.Type == JTokenType.Array)
                {
                    RemoveFromArray((JArray)((JProperty)jObject).Value, props);
                }
                else if (jObject.Type == JTokenType.String)
                {
                    break;
                }
                else
                {
                    RemoveIds((JObject)jObject, props);
                }
            }
        }

        public static string RemoveUnwantedProperties(string jsonString,bool isFamilyCard)
        {

            #region old foreach logic
            //var jsonObject = JObject.Parse(jsonString);
            //foreach (var offer in jsonObject["offers"])
            //{
            //    ProcessPrices(offer["prices"] as JObject);

            //    // Process each travelerPassOffer within the offer
            //    var travelerPassOffers = offer["travelerPassOffers"] as JArray;
            //    if (travelerPassOffers != null)
            //    {
            //        foreach (var travelerPassOffer in travelerPassOffers)
            //        {
            //            ProcessPrices(travelerPassOffer["prices"] as JObject);
            //        }
            //    }
            //}
            #endregion

            var jsonObject = JObject.Parse(jsonString);
            var offers = jsonObject["offers"] as JArray;

            // Iterate backward to safely remove items from the collection while iterating
            for (int i = offers.Count - 1; i >= 0; i--)
            {
                JObject offer = (JObject)offers[i];

                // Get the 'tags' array
                JArray tags = (JArray)offer["tags"];
                if(tags != null && isFamilyCard == true)
                {
                    if (!tags.Any(tag => tag.ToString() == "family-card"))
                    {                       
                        offers.RemoveAt(i);
                        continue; // Skip processing this offer
                    }
                }  
                else if(tags != null && isFamilyCard == false)
                {
                    if (tags.Any(tag => tag.ToString() == "family-card"))
                    {                        
                        offers.RemoveAt(i);
                        continue; // Skip processing this offer
                    }
                }

                // Process the prices for the current offer
                ProcessPrices(offer["prices"] as JObject);

                // Process each travelerPassOffer within the offer
                var travelerPassOffers = offer["travelerPassOffers"] as JArray;
                if (travelerPassOffers != null)
                {
                    foreach (var travelerPassOffer in travelerPassOffers)
                    {
                        ProcessPrices(travelerPassOffer["prices"] as JObject);
                    }
                }
            }


            string modifiedJsonString = jsonObject.ToString(Formatting.Indented);
            return modifiedJsonString;
        }

        static void ProcessPrices(JObject prices)
        {
            if (prices != null)
            {
                prices.Remove("billings");
                

                var selling = prices["selling"] as JObject;
                if (selling != null)
                {
                    var newSelling = new JObject();

                    var agentSellingPrice = selling["agentSellingPrice"];
                    if (agentSellingPrice != null)
                    {
                        newSelling["Total"] = agentSellingPrice;
                    }

                    var partnerCommission = selling["partnerCommission"];
                    if (partnerCommission != null)
                    {
                        newSelling["Discount"] = partnerCommission;
                    }

                    var agentCommission = selling["agentCommission"];
                    if (agentCommission != null)
                    {
                        newSelling["agentCommission"] = agentCommission;
                    }

                    prices["selling"] = newSelling;
                }
                else
                {
                    prices.Remove("selling");
                }
            }
        }

    }
}
