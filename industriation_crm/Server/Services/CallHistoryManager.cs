using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public List<call_history> GetCallHistoryByContactIds(List<int?> contactsIds)
        {
            try
            {
                List<call_history> call_Histories = _dbContext.call_history.Where(c => contactsIds.Contains(c.contact_id)).ToList();
                return call_Histories;
            }
            catch
            {
                throw;
            }
        }

        public CallHistoryReturnData GetCallHistoryDetails(CallHistoryFilter callHistoryFilter)
        {
            CallHistoryReturnData callHistoryReturnData = new CallHistoryReturnData();
            

            try
            {
                var query = _dbContext.call_history.Where(c => c.id != 0);
                if (!String.IsNullOrEmpty(callHistoryFilter.type))
                    query = query.Where(c => c.type!.Contains(callHistoryFilter.type));
                if(!String.IsNullOrEmpty(callHistoryFilter.phone))
                    query = query.Where(c => c.client_number!.Contains(callHistoryFilter.phone!));
                if (callHistoryFilter.managers != null && callHistoryFilter.managers.Count() != 0)
                    query = query.Where(c => callHistoryFilter.managers.Contains(c.user_id));
                if (callHistoryFilter.call_date_from != null)
                    query = query.Where(c => c.date_time > callHistoryFilter.call_date_from);
                if (callHistoryFilter.call_date_to != null)
                    query = query.Where(c => c.date_time < callHistoryFilter.call_date_to);
                callHistoryReturnData.count = query.Count();

                callHistoryReturnData.call_historyes = query
                    .Include(c=>c.contact).Include(c=>c.user)
                    .OrderByDescending(o => o.date_time)
                        .Skip(callHistoryFilter.calls_on_page * (callHistoryFilter.current_page - 1)).Take(callHistoryFilter.calls_on_page).ToList();
                return callHistoryReturnData;
            }
            catch
            {
                throw;
            }
        }
    }
}
