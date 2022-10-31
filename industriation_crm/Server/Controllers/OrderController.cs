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
        private Integration1C integration1C = new();
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
            await this.hubContext.Clients.All.UpdateStatus("1");


            integration1C.AddNewOrderPay(Complete1CData(order));
            return Ok();
        }
        private _1COrderPay Complete1CData(order order)
        {
            _1COrderPay _1COrderPay = new _1COrderPay();
            _1COrderPay.id = order.id.ToString();

            _1COrderPay.contragent = new _1CContragent();
            _1COrderPay.products = new List<_1CProduct>();

            _1COrderPay.contragent.inn = order?.client?.org_inn.ToString();
            _1COrderPay.contragent.kpp = order?.client?.org_kpp.ToString();
            _1COrderPay.contragent.name_in_programm = order?.client?.org_name;
            _1COrderPay.contragent.ogrn = order?.client?.org_ogrn.ToString();
            foreach(var p in order?.product_To_Orders!)
            {
                _1CProduct _1CProduct = new _1CProduct();
                _1CProduct.price = p?.product_price?.ToString();
                _1CProduct.summ = p?.total_price?.ToString();
                _1CProduct.count = p?.count?.ToString();
                _1CProduct.article = p?.id.ToString(); //POMENIAT na article;
                _1CProduct.name = p?.product?.name;
                _1COrderPay.products.Add(_1CProduct);
            }

            return _1COrderPay;
        }
        
    }
}
