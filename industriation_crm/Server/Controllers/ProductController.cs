using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _IProduct;
        public ProductController(IProduct IProduct)
        {
            _IProduct = IProduct;
        }
        [HttpGet]
        public async Task<List<product>> Get()
        {
            return await Task.FromResult(_IProduct.GetProductDetails());
        }
        [HttpPost]
        public void Post(product product)
        {
            _IProduct.AddProduct(product);
        }
        [HttpPut]
        public void Put(product product)
        {
            _IProduct.UpdateProductDetails(product);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            product product = _IProduct.GetProductData(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }
    }
}
