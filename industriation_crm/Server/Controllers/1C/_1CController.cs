using industriation_crm.Server._1C;
using industriation_crm.Server.Interfaces;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace industriation_crm.Server.Controllers._1C
{
    public class Pay
    {
        public string orderId { get; set; }
        public string paySumm { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class _1CController : ControllerBase
    {
        private readonly IOrder _IOrder;
        private Integration1C integration1C = new();
        private readonly IHubContext<StatusNotificationHub, IStatusNotification> hubContext;
        public _1CController(IOrder IOrder, IHubContext<StatusNotificationHub, IStatusNotification> hubContext)
        {
            _IOrder = IOrder;
            this.hubContext = hubContext;
        }
        [HttpGet("{orderId}")]
        public IActionResult CreateNewBuyerCheck(int orderId)
        {
            order order = _IOrder.GetOrderData(orderId);
            if (order != null)
            {
                integration1C.AddNewOrderPay(Complete1CData(order));
                return Ok(order);
            }
            return NotFound();

        }
        [HttpPost]
        public async void CreatePayment([FromBody] Pay pay)
        {
            order order = _IOrder.GetOrderData(Convert.ToInt32(pay.orderId));
            order_pay order_Pay = new order_pay();
            order_Pay.price = Convert.ToDouble(pay.paySumm);
            order.order_Pays?.Add(order_Pay);
            _IOrder.UpdateOrderDetails(order);
            await this.hubContext.Clients.All.UpdateStatus("1");
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
            foreach (var p in order?.product_To_Orders!)
            {
                _1CProduct _1CProduct = new _1CProduct();
                _1CProduct.price = p?.product_price?.ToString();
                _1CProduct.summ = p?._total_price?.ToString();
                _1CProduct.count = p?.count?.ToString();
                _1CProduct.article = p?.id.ToString(); //POMENIAT na article;
                _1CProduct.name = p?.product?.name;
                _1COrderPay.products.Add(_1CProduct);
            }

            return _1COrderPay;
        }
    }
}
