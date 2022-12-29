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
            return Ok();

        }
        [HttpPost("CreateSupplierOrder")]
        public void CreateSupplierOrder(supplier_order supplier_Order)
        {
            if(supplier_Order != null)
            {
                _1CSupplierOrder _1CSupplierOrder = new _1CSupplierOrder();
                _1CSupplierOrder.id = supplier_Order.id.ToString();
                _1CSupplierOrder.products = Complete1CProductData(supplier_Order?.product_to_orders!);
                _1CSupplierOrder.contragent = Complete1CContragentData(supplier_Order?.supplier!);
                integration1C.AddNewSupplierOrder(_1CSupplierOrder);
            }
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
            _1COrderPay.contragent = Complete1CContragentData(order?.client!);
            _1COrderPay.products = Complete1CProductData(order?.product_To_Orders!);

            return _1COrderPay;
        }

        private _1CContragent Complete1CContragentData(client client)
        {
            _1CContragent contragent = new _1CContragent();
            contragent.inn = client?.org_inn.ToString();
            contragent.kpp = client?.org_kpp.ToString();
            contragent.name_in_programm = client?.org_name;
            contragent.ogrn = client?.org_ogrn.ToString();
            contragent.bik = client?.bank_bik.ToString();
            contragent.ras_schet = client?.bank_ras_schet.ToString();

            return contragent;
        }

        private List<_1CProduct> Complete1CProductData(List<product_to_order> product_To_Orders)
        {
            List<_1CProduct> _1CProducts = new List<_1CProduct>();
            foreach (var p in product_To_Orders!)
            {
                _1CProduct _1CProduct = new _1CProduct();
                _1CProduct.price = p?.supplier_price?.ToString();
                _1CProduct.summ = (p?.supplier_price * p?.count).ToString();
                _1CProduct.count = p?.count?.ToString();
                _1CProduct.article = p?.product?.article!.ToString(); //POMENIAT na article;
                _1CProduct.name = p?.product?.name;
                _1CProducts.Add(_1CProduct);
            }
            return _1CProducts;
        }
    }
}
