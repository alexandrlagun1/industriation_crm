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
            if (callHistoryFilter.call_date_from == null)
                callHistoryFilter.call_date_from = DateTime.MinValue;
            if (callHistoryFilter.call_date_to == null)
                callHistoryFilter.call_date_to = DateTime.MaxValue;

            try
            {

                callHistoryReturnData.count = _dbContext.call_history.Where(c => c.type!.Contains(callHistoryFilter.type!) && callHistoryFilter.managers!.Contains(c.user) && c.client_number!.Contains(callHistoryFilter.phone!) && c.date_time >= callHistoryFilter.call_date_from && c.date_time <= callHistoryFilter.call_date_to).Count();

                callHistoryReturnData.call_historyes = _dbContext.call_history.Where(c=>c.type!.Contains(callHistoryFilter.type!) && callHistoryFilter.managers!.Contains(c.user) && c.client_number!.Contains(callHistoryFilter.phone!) && c.date_time >= callHistoryFilter.call_date_from && c.date_time <= callHistoryFilter.call_date_to)
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
