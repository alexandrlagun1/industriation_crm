using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddProductOfOrderToSupplierOrderController : ControllerBase
    {
        private readonly IProductToOrder _IProductToOrder;
        public AddProductOfOrderToSupplierOrderController(IProductToOrder IProductToOrder)
        {
            _IProductToOrder = IProductToOrder;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<product_to_order> ProductsOfSupplierOrder = _IProductToOrder.GetProductsOfNoSupplierOrder();
            if (ProductsOfSupplierOrder != null)
            {
                return Ok(ProductsOfSupplierOrder);
            }
            return NotFound();
        }
        [HttpPut]
        public void Put(List<product_to_order> product_to_orders)
        {
            _IProductToOrder.UpdateProductsToOrder(product_to_orders);
        }
    }
}
