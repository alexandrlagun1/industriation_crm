using industriation_crm.Server._1C;
using industriation_crm.Server.Interfaces;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _IOrder;
        private readonly IHubContext<StatusNotificationHub, IStatusNotification> hubContext;
       
        public OrderController(IOrder IOrder, IHubContext<StatusNotificationHub, IStatusNotification> statusHub)
        {
            _IOrder = IOrder;
            this.hubContext = statusHub;
        }
        [HttpGet]
        public async Task<List<order>> Get()
        {
            return await Task.FromResult(_IOrder.GetOrderDetails());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            order order = _IOrder.GetOrderData(id);
            if (order != null)
            {
                return Ok(order);
            }
            return NotFound();
        }
        [HttpPost]
        public int Post(order order)
        {
            return _IOrder.AddOrder(order);
        }
        [HttpPut]
        public void Put(order order)
        {
            _IOrder.UpdateOrderDetails(order);
        }
        [HttpPut("bill")]
        public async Task<IActionResult> Bill(order order)
        {
            _IOrder.UpdateOrderDetails(order);
            


            
            return Ok();
        }
        
        
    }
}
