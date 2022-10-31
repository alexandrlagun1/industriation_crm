using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsToSupplierOrderController : ControllerBase
    {
        private readonly IProductToOrder _IProductToOrder;
        public ProductsToSupplierOrderController(IProductToOrder IProductToOrder)
        {
            _IProductToOrder = IProductToOrder;
        }
        [HttpGet("{supplierOrderId}")]
        public IActionResult Get(int supplierOrderId)
        {
            List<product_to_order> ProductsOfSupplierOrder = _IProductToOrder.GetProductsOfSupplierOrder(supplierOrderId);
            if (ProductsOfSupplierOrder != null)
            {
                return Ok(ProductsOfSupplierOrder);
            }
            return NotFound();
        }
    }
}
