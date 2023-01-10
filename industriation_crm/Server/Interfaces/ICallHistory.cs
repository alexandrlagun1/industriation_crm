using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ICallHistory
    {
        public void AddCallHistory(call_history call_history);
        public CallHistoryReturnData GetCallHistoryDetails(CallHistoryFilter callHistoryFilter);
    }
}
