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
                _1CSupplierOrder.products = Complete1CSupplierProductData(supplier_Order?.product_to_orders!);
                _1CSupplierOrder.contragent = Complete1CContragentData(supplier_Order?.supplier!);
                integration1C.AddNewSupplierOrder(_1CSupplierOrder);
            }
        }
        [HttpPost]
        public async void CreatePayment([FromBody] Pay pay)
        {

            
            string pay_summ = pay.paySumm.Replace(" ", "");
            order order = _IOrder.GetOrderData(Convert.ToInt32(pay.orderId));
            
            order_pay order_pay = new order_pay();
            order_pay.date = DateTime.Now;
            try
            {
                order_pay.price = Convert.ToDouble(pay_summ.Replace(",", "."));
            }
            catch
            {
                order_pay.price = Convert.ToDouble(pay_summ.Replace(".",","));
            }
            order.order_Pays?.Add(order_pay);
            if (order.order_status_id == 1 && order.pay_conditions == 1)
            {
                double? order_pays_summ = order.order_Pays?.Select(p => p.price).Sum();
                Console.WriteLine($"order_pays_summ = {order_pays_summ}");
                double? min_predoplata = order.price_summ / 100 * order.pay_predoplata_percent;
                Console.WriteLine($"min_predoplata = {min_predoplata}");
                if (order_pays_summ + 1 >= min_predoplata)
                    order.order_status_id = 3;
            }
            _IOrder.UpdateOrderDetails(order);

            megafon_info megafon_Info = new megafon_info();
            megafon_Info.cmd = "1";

            order_pay.order = null;
            await this.hubContext.Clients.User(order.user_id.ToString()).AddNewPay(order_pay);
        }
        
        private _1COrderPay Complete1CData(order order)
        {
            _1COrderPay _1COrderPay = new _1COrderPay();
            _1COrderPay.id = order.id.ToString();
            _1COrderPay.contragent = Complete1CContragentData(order?.client!);
            _1COrderPay.products = Complete1COrderProductData(order?.product_To_Orders!);

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

        private List<_1CProduct> Complete1COrderProductData(List<product_to_order> product_To_Orders)
        {
            List<_1CProduct> _1CProducts = new List<_1CProduct>();
            foreach (var p in product_To_Orders!)
            {
                _1CProduct _1CProduct = new _1CProduct();
                _1CProduct.price = (p?._total_price / p?.count).ToString();
                _1CProduct.summ = p?._total_price.ToString();
                _1CProduct.count = p?.count?.ToString();
                _1CProduct.article = p?.product?.article!.ToString(); //POMENIAT na article;
                _1CProduct.name = p?.product?.name;
                _1CProducts.Add(_1CProduct);
            }
            return _1CProducts;
        }
        private List<_1CProduct> Complete1CSupplierProductData(List<product_to_order> product_To_Orders)
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
