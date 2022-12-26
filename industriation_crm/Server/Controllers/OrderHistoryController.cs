using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly IOrderHistory _IOrderHistory;
        public OrderHistoryController(IOrderHistory IOrderHistory)
        {
            _IOrderHistory = IOrderHistory;
        }
        [HttpPost]
        public void Post(order_history order_history)
        {
            _IOrderHistory.AddOrderHistory(order_history);
        }
    }
}
