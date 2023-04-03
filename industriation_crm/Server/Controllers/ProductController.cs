using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using industriation_crm.Shared.FilterModels;
using Microsoft.AspNetCore.Mvc;
using industriation_crm.Server.Retail;

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
        [HttpGet("GetProductDetails/{categoryId}")]
        public async Task<List<product>> GetProductDetails(int categoryId)
        {
            return await Task.FromResult(_IProduct.GetProductDetails(categoryId));
        }
        [HttpPost("FindByFilter")]
        public async Task<ProductReturnData> FindByFilter([FromBody]ProductFilter productFilter)
        {
            return await Task.FromResult(_IProduct.GetFromFilter(productFilter));
        }
        [HttpPost]
        public void Post(product product)
        {
            _IProduct.AddProduct(product);
            RetailOrderCreator.AddProduct(product);
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
