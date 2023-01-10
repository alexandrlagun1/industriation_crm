using industriation_crm.Server.Interfaces;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using Ubiety.Dns.Core;

namespace industriation_crm.Server.Controllers.Megafon
{
    [Route("api/[controller]")]
    [ApiController]
    public class MegafonController : ControllerBase
    {
        private readonly ICallHistory _ICallHistory;
        private readonly IHubContext<StatusNotificationHub, IStatusNotification> hubContext;
        private readonly IUser _IUser;
        private readonly IContact _IContact;
        public MegafonController(ICallHistory ICallHistory, IHubContext<StatusNotificationHub, IStatusNotification> hubContext, IUser iUser, IContact iContact)
        {
            _ICallHistory = ICallHistory;
            this.hubContext = hubContext;
            _IUser = iUser;
            _IContact = iContact;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int user_id, string phone)
        {
            var user = _IUser.GetUserData(user_id);
            var data = new[]
            {
                new KeyValuePair<string, string>("phone", phone),
                new KeyValuePair<string, string>("user", user.megafon_login!),
            };
            using (var client = new HttpClient())
            {
                var urlEncoded = new FormUrlEncodedContent(data);
                urlEncoded.Headers.Add("X-API-KEY", "34258b4d-4561-4c81-bc79-8b437c741300");
                var answer = await client.PostAsync("https://vats555687.megapbx.ru/crmapi/v1/makecall", urlEncoded);
            }
            
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] megafon_info _info)
        {
            var user = _IUser.GetUserDataByPhone(_info.telnum!);
            var contact = _IContact.GetContactDataByPhone(_info.phone!);
            if (contact != null)
            {
                contact!.client!.contacts = null;
                contact!.client!.user = null;
                _info.contact = contact;
            }
            if (_info.cmd == "history")
            {
                call_history call_History = new call_history();
                call_History.client_number = _info.phone;
                call_History.manager_number = _info.telnum;
                call_History.call_id = _info.callid;
                call_History.duration = _info.duration;
                call_History.status = _info.status;
                call_History.type = _info.type;
                call_History.date_time = DateTime.Now;
                call_History.record = _info.link;
                call_History.user_id = user?.id;
                call_History.contact_id = contact?.id;
                _ICallHistory.AddCallHistory(call_History);
            }
            if (_info.cmd == "event")
            {
                _info.date_time = DateTime.Now;
                
                if (user != null)
                    await this.hubContext.Clients.User(user.id.ToString()).MegafonCall(_info);
            }
            return Ok();
        }
    }
}
