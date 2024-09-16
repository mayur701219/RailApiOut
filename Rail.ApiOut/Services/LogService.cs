using Newtonsoft.Json;
using Rail.ApiOut.IServices;
using Rail.BO.Entities;

namespace Rail.ApiOut.Services
{
    public class LogService : ILogService
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private string baseUrl = string.Empty;
        public LogService(IHelper helper, IConfiguration configuration)
        {
            _helper = helper;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("Links").GetSection("baseUrl").Value;
        }
        public void Log(ErrorLogsModel logsModel)
        {
            try
            {
                _helper.ExecuteAPI(baseUrl + "/api/Logs/LogError", JsonConvert.SerializeObject(logsModel), HttpMethod.Post);
            }
            catch (Exception ex) { }
        }
    }
}
