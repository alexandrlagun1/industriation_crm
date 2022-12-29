using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Services
{
    public class CallHistoryManager : ICallHistory
    {
        readonly DatabaseContext _dbContext = new();
        public CallHistoryManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCallHistory(call_history call_history)
        {
            try
            {
                _dbContext.call_history.Add(call_history);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
