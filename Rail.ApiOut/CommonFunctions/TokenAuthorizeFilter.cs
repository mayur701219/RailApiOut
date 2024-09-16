using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Rail.ApiOut.IServices;

namespace Rail.ApiOut.CommonFunctions
{
    public class TokenAuthorizeFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IAuthService _authorizationService;

        public TokenAuthorizeFilter(IAuthService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var actdescr = (context.ActionDescriptor as ControllerActionDescriptor);
            if (actdescr != null)
            {
                var attrs = actdescr.MethodInfo.GetCustomAttributes(typeof(DisableAttr), true);
                if (attrs.Any())
                    return;
            }
            string token = string.Empty;
            try
            {
                token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!_authorizationService.AuthenticateToken(token))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch (Exception) { }
        }
    }
}
