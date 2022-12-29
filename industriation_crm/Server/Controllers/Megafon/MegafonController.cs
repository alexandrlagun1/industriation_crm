using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers.Megafon
{
    public class Info
    {
        public string? phone { get; set; }
        public string? cmd { get; set; }
        public string? crm_token { get; set; }
        public string? callid { get; set; }
        public string? diversion { get; set; }
        public string? type { get; set; }
        public string? user { get; set; }
        public string? ext { get; set; }
        public string? telnum { get; set; }
        public string? direction { get; set; }
        public string? duration { get; set; }
        public string? status { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MegafonController : ControllerBase
    {
        private readonly ICallHistory _ICallHistory;
        public MegafonController(ICallHistory ICallHistory)
        {
            _ICallHistory = ICallHistory;
        }
        [HttpPost]
        public IActionResult Post([FromForm] Info _info)
        {
            if(_info.cmd == "history")
            {
                call_history call_History = new call_history();
                call_History.from_number = _info.phone;
                call_History.to_number = _info.telnum;
                call_History.call_id = _info.callid;
                call_History.duration = _info.duration;
                call_History.status = _info.status;
                call_History.type = _info.type;
                _ICallHistory.AddCallHistory(call_History);
            }
            return Ok();
        }
    }
}
