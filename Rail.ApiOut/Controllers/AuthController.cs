using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using Rail.BO.ApiOutModels;
using Rail.BO.MiscModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Rail.ApiOut.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    //[ServiceFilter(typeof(TokenAuthorizeFilter))]
    public class AuthController : ControllerBase
    {
        IConfiguration _config;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }
        [HttpPost]
        //[DisableAttr]
        public IActionResult Auth([FromBody] UserModel user)
        {
            IActionResult response = Unauthorized();
            if (user != null)
            {
                string MobileNo = string.Empty;
                string AgentId = string.Empty;
                if (_authService.IsUserValid(user.UserName, user.Password, ref MobileNo, ref AgentId))
                {
                    var issuer = _config["Jwt:Issuer"];
                    var audience = _config["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );
                    var subject = new ClaimsIdentity(new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, MobileNo),
                        new Claim(JwtRegisteredClaimNames.NameId, AgentId),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Correlation),// co-relation id
                        new Claim("Currency", "INR"),
                        });
                    var expires = DateTime.UtcNow.AddMinutes(120);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials,
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);

                    var data = new
                    {
                        jwtToken,
                        expiration = TimeZoneInfo.ConvertTimeFromUtc(expires, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
                    };

                    ApiResponse apiresponse = new ApiResponse
                    {
                        Success = true, // Assuming false due to validation error
                        Message = data, // Setting Message from the label field
                        Errors = null // Setting Errors from the details array
                    };

                    //return Ok(_authService.SaveToken(jwtToken));
                    return new JsonResult(apiresponse);
                }
            }
            return response;
        }
    }
}
