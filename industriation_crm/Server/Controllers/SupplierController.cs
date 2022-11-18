using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplier _ISupplier;
        public SupplierController(ISupplier ISupplier)
        {
            _ISupplier = ISupplier;
            
        }
        [HttpGet]
        public async Task<List<supplier>> Get()
        {
            return await Task.FromResult(_ISupplier.GetSupplierDetails());

        }
        [HttpPost]
        public void Post(supplier supplier)
        {
            _ISupplier.AddSupplier(supplier);
        }
        [HttpPut]
        public void Put(supplier supplier)
        {
            _ISupplier.UpdateSupplierDetails(supplier);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _ISupplier.DeleteSupplier(id);
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            supplier supplier = _ISupplier.GetSupplierData(id);
            if (supplier != null)
            {
                return Ok(supplier);
            }
            return NotFound();
        }
    }
}
