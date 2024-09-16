using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using Rail.BO.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Rail.ApiOut.Services
{
    public class AuthService : IAuthService
    {
        private readonly RailDBContext _db;
        public AuthService(RailDBContext db)
        {
            _db = db;
        }
        public bool AuthenticateToken(string token)
        {
            try
            {
                //return _db.tokens.Any(x => x.token == token && DateTime.Compare(x.expiration.Value, DateTime.Now) > 0);
                return _db.tokens.Any(x => x.token == token && x.expiration.Value > DateTime.Now);
            }
            catch (Exception ex) { throw; }
        }
        public object SaveToken(string token)
        {
            DateTime now = DateTime.Now.AddMinutes(30);
            try
            {
                var tokens = _db.tokens.FirstOrDefault(x => x.token == token);
                if (tokens != null)
                {
                    tokens.token = token;
                    tokens.expiration = now;
                }
                else
                {
                    _db.tokens.Add(new TokensModel
                    {
                        expiration = now,
                        token = token
                    });
                }
                _db.SaveChanges();
            }
            catch (Exception ex) { throw; }
            return new { token = token, expiration = now };
        }
        public bool IsUserValid(string UserName, string Password, ref string? MobileNo, ref string AgentId)
        {
            bool IsExist = false;
            try
            {
                IsExist = _db.agents.Any(x => x.UserName == UserName && x.Password == Password);
                if (IsExist)
                {
                    var currentAgent = _db.agents.FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
                    MobileNo = currentAgent?.MobileNumber ?? currentAgent?.MobileNumber;
                    AgentId = currentAgent.UserID.ToString();
                }
            }
            catch (Exception ex) { throw; }
            return IsExist;
        }
    }
}
