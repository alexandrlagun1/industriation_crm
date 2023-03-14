using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatus _IOrderStatus;
        public OrderStatusController(IOrderStatus IOrderStatus)
        {
            _IOrderStatus = IOrderStatus;
        }
        [HttpGet]
        public List<order_status> Get()
        {
            List<order_status> order_Statuses = _IOrderStatus.GetOrderStatusDetails();
            return order_Statuses;
        }
    }
}
