using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierOrderController : ControllerBase
    {
        private readonly ISupplierOrder _ISupplierOrder;
        public SupplierOrderController(ISupplierOrder ISupplierOrder)
        {
            _ISupplierOrder = ISupplierOrder;
        }
        [HttpGet]
        public async Task<List<supplier_order>> Get()
        {
            return await Task.FromResult(_ISupplierOrder.GetSupplierOrderDetails());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            supplier_order supplier_order = _ISupplierOrder.GetSupplierOrderData(id);
            if (supplier_order != null)
            {
                return Ok(supplier_order);
            }
            return NotFound();
        }
        [HttpPut]
        public void Put(supplier_order supplier_order)
        {
            _ISupplierOrder.UpdateSupplierOrderDetails(supplier_order);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _ISupplierOrder.DeleteSupplierOrder(id);
            return Ok();
        }

        [HttpPost]
        public void Post(supplier_order supplier_order)
        {
            _ISupplierOrder.AddSupplierOrder(supplier_order);
        }
    }
}
