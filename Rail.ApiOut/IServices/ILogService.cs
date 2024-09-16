using Rail.BO.Entities;

namespace Rail.ApiOut.IServices
{
    public interface ILogService
    {
        void Log(ErrorLogsModel logsModel);
    }
}
