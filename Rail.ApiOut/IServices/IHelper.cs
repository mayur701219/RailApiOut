namespace Rail.ApiOut.IServices
{
    public interface IHelper
    {
        Task<string> ExecuteAPI(string url, string data, HttpMethod httpMethod);
        Task<string> ExecuteGetAPIFlat(string url);
        string GetValueFromClaims(string token, string proptype);
        void SendExeptionMail(Exception ex);
    }
}
