using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductToOrderController : ControllerBase
    {
        private readonly IProductToOrder _IProductToOrder;
        public ProductToOrderController(IProductToOrder IProductToOrder)
        {
            _IProductToOrder = IProductToOrder;
        }
        [HttpGet]
        public async Task<List<product_to_order>> Get(int order_id)
        {
            return await Task.FromResult(_IProductToOrder.GetProductToOrdertDetails(order_id));
        }
        [HttpPost]
        public product_to_order Post(product_to_order product_to_order)
        {
            product_to_order = _IProductToOrder.AddProductToOrder(product_to_order);
            return product_to_order;
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IProductToOrder.DeleteProductToOrder(id);
            return Ok();
        }
        [HttpPost("remove_products_to_orders")]
        public IActionResult RemoveProductToOrders(List<product_to_order> product_to_orders)
        {
            _IProductToOrder.DeleteProductToOrderInRange(product_to_orders);
            return Ok();
        }
        [HttpPost("add_products_to_order")]
        public IActionResult AddProductsToOrder(List<product_to_order> product_to_orders)
        {
            _IProductToOrder.AddProductToOrderInRange(product_to_orders);
            return Ok();
        }
        [HttpPut("update_products_to_order")]
        public IActionResult UpdateProductsToOrder(List<product_to_order> product_to_orders)
        {
            _IProductToOrder.UpdateProductsToOrder(product_to_orders);
            return Ok();
        }
    }
}
