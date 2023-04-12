using industriation_crm.Server._1C;
using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Retail;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.FilterModels;
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
        [HttpGet("GetOrdersByClientId/{clientId}")]
        public async Task<List<order>> GetOrdersByClientId(int clientId)
        {
            List <order> orders = await Task.FromResult(_IOrder.GetOrdersByClientId(clientId));
            return orders;
        }
        [HttpPost("GetOrders")]
        public async Task<OrdersReturnData> GetOrders([FromBody] OrdersFilter ordersFilter)
        {
            return await Task.FromResult(_IOrder.GetOrderDetails(ordersFilter));
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

            int order_id = _IOrder.AddOrder(order);
            if (order?.retail_synchro == true && order?.user_id != 17)
                RetailOrderCreator.CreateOrder(order);
            return order_id;
        }
        [HttpPut]
        public void Put(order order)
        {
            if (order?.retail_synchro == true)
                RetailOrderCreator.UpdateOrder(order);
            _IOrder.UpdateOrderDetails(order, true);
            
        }
        [HttpPut("bill")]
        public async Task<IActionResult> Bill(order order)
        {
            _IOrder.UpdateOrderDetails(order, true);
            return Ok();
        }


    }
}
