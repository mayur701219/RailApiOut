namespace Rail.ApiOut.IServices
{
    public interface IAuthService
    {
        bool AuthenticateToken(string token);
        object SaveToken(string token);
        bool IsUserValid(string UserName, string Password, ref string MobileNo, ref string AgentId);
    }
}
