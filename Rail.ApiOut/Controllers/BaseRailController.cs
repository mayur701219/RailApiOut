using Microsoft.AspNetCore.Mvc;
using Rail.ApiOut.IServices;
using Rail.BO.ApiOutModels;

namespace Rail.ApiOut.Controllers
{
    public class BaseRailController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHelper _helper;
        public BaseRailController(IHttpContextAccessor httpContextAccessor, IHelper helper)
        {
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
        }
        public string _AgentID => _helper.GetValueFromClaims(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""), "nameid");
        public string _correlation => _httpContextAccessor.HttpContext.Request.Headers["correlationId"];
        
        //public string _correlation => _helper.GetValueFromClaims(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""), "unique_name");
        public string _APIKEY => _httpContextAccessor.HttpContext.Request.Headers["APIKEY"];
        public string _Currency => _helper.GetValueFromClaims(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""), "Currency");        
    }
}
