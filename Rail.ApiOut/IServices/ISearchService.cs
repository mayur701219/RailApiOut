using Rail.BO.ApiOutModels;

namespace Rail.ApiOut.IServices
{
    public interface ISearchService
    {
        Task SaveHistory(string SearchId, string CorrelationId, string Type, string Response, long AgentId);
        Task<List<SearchHistoryModel>> GetSearch(List<string> SearchIds);
        Task<List<SearchHistoryModel>> GetSearchHistory(string correlationId);
        //void LogRequestToDatabase(Guid logId, string request);
    }
}
