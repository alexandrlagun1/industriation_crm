using industriation_crm.Server.Interfaces;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallHistoryController : ControllerBase
    {
        private readonly ICallHistory _ICallHistory;
        public CallHistoryController(ICallHistory ICallHistory)
        {
            _ICallHistory = ICallHistory;
        }
        [HttpPost]
        public async Task<CallHistoryReturnData> GetCallHistoryes([FromBody] CallHistoryFilter callHistoryFilter)
        {
            return await Task.FromResult(_ICallHistory.GetCallHistoryDetails(callHistoryFilter));
        }
        [HttpPost("GetCallHistoryByClientId")]
        public async Task<List<call_history>> GetCallHistoryByClientId(List<int?> clientIds)
        {
            return await Task.FromResult(_ICallHistory.GetCallHistoryByContactIds(clientIds));
        }
    }
}
