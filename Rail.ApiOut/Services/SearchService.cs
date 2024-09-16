using Microsoft.EntityFrameworkCore;
using Rail.ApiOut.CommonFunctions;
using Rail.ApiOut.IServices;
using Rail.BO.ApiOutModels;

namespace Rail.ApiOut.Services
{
    public class SearchService : ISearchService
    {
        private readonly RailDBContext _db;
        public SearchService(RailDBContext db)
        {
            _db = db;
        }
        public async Task SaveHistory(string SearchId, string CorrelationId, string Type, string Response, long AgentId)
        {
            SearchHistoryModel model = new SearchHistoryModel();
            try
            {
                model.SearchId = SearchId;
                model.CorrelationId = CorrelationId;
                model.Type = Type;
                model.Response = Response;
                model.AgentId = AgentId;
                await _db.history.AddAsync(model);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<SearchHistoryModel>> GetSearch(List<string> SearchIds)
        {
            List<SearchHistoryModel> model = new List<SearchHistoryModel>();
            try
            {
                model = await _db.history.Where(x => SearchIds.Contains(x.SearchId)).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public async Task<List<SearchHistoryModel>> GetSearchHistory(string correlationId)
        {
            List<SearchHistoryModel> model = new List<SearchHistoryModel>();
            try
            {
                model = await _db.history.Where(x => x.CorrelationId == correlationId).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }


        //public async void LogRequestToDatabase(Guid logId, string request)
        //{

        //}

       
    }
}
