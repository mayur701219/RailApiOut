using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace Rail.ApiOut.Services
{
    public class Helper : IHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly RailDBContext _db;
        private readonly IConfiguration _configuration;
        public Helper(HttpClient httpClient, IHttpContextAccessor context, RailDBContext db, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _db = db;
            _configuration = configuration;
        }
        async Task<string> IHelper.ExecuteAPI(string url, string data, HttpMethod httpMethod)
        {
            string _data = string.Empty;
            try
            {
                var request = CreateRequest(url, data, httpMethod);
                request.Headers.Add("X-PRODUCTTYPE", await GetProductType());
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    _data = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception) { throw; }
            return _data;
        }
        async Task<string> IHelper.ExecuteGetAPIFlat(string url)
        {
            string _data = string.Empty;
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                _data = await response.Content.ReadAsStringAsync();
            }
            return _data;
        }
        string IHelper.GetValueFromClaims(string token,string proptype)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return "";
            }

            return jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == proptype).Value;
        }
        private HttpRequestMessage CreateRequest(string url, string data, HttpMethod httpMethod)
        {
            return new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(url, UriKind.Relative),
                Content = new StringContent(data)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
        }
        private async Task<string> GetProductType()
        {
            string productType = string.Empty;
            try
            {
                //string CookieID = _context.HttpContext.Request.Cookies["ASP.NET_SessionId"].ToString();
                //productType = await _sessionHelper.GetSessionRail("BaseCurrency", CookieID);
                //productType = (productType != "INR") ? "INT" : "DOM";
                productType = "DOM";
            }
            catch (Exception ex)
            {
                productType = "DOM";
            }
            return productType;
        }

        public  void SendExeptionMail(Exception ex)
        {
            string FromMail = "no-reply@riya.travel";
            string displayName = "Rail ApiOut";
            string ToEmail = "mayur.riya04@gmail.com";
            string body = "Hello,\r\n\r\n" +
                "An exception has occurred in RailApiOut at " + DateTime.Now + ".\r\n\r\n" +
                "Exception Details:\r\n" +
                "-----------------------------------------\r\n" +
                "Exception Type: " + ex.GetType().Name + "\r\n" +
                "Message: " + ex.Message + "\r\n" +
                "Source: " + ex.Source + "\r\n" +
                "Stack Trace: " + ex.StackTrace + "\r\n\r\n";

            string subject = "Exception in RailApiOut";
            MailAddress sendFrom = new MailAddress(FromMail, displayName);
            MailAddress sendTo = new MailAddress(ToEmail);
            MailMessage nmsg = new MailMessage(sendFrom, sendTo);

            nmsg.Subject = subject;
            nmsg.IsBodyHtml = false;
            nmsg.Body = body;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = "smtp.pepipost.com";
            smtpClient.Port = 25;
            string smtpUserName = "riyatravelpepi@riya.travel";
            string smtpPassword = "7b79dc!db222a";
            smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
            smtpClient.EnableSsl = false;
            smtpClient.Send(nmsg);
        }
    }
}
