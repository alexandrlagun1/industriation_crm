using industriation_crm.Server.SignalRNotification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace industriation_crm.Server.Controllers.Statuses
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IHubContext<StatusNotificationHub, IStatusNotification> hubContext;

        public StatusController(IHubContext<StatusNotificationHub, IStatusNotification> statusHub)
        {
            this.hubContext = statusHub;
        }

        [HttpGet("{status}")]
        public async Task<IActionResult> UpdateStatus(string status)
        {
            //await this.hubContext.Clients.All.UpdateStatus(status);
            return Ok();
        }
    }
}
