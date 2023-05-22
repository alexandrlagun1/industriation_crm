using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using industriation_crm.Shared.FilterModels;
using Microsoft.AspNetCore.Mvc;
using industriation_crm.Server.Retail;
using industriation_crm.Shared.Img;
using industriation_crm.Server.Queues;
using industriation_crm.Shared.industriation_site_model;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using industriation_crm.Server.Models;
using Serilog;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BackgroundWorkerQueue _backgroundWorkerQueue;
        private readonly BackgroundPriceQueue _backgroundPriceQueue;
        private readonly BackgroundRemoveProductQueue _backgroundRemoveProductQueue;
        private readonly IProduct _IProduct;
        private readonly DatabaseContext _dbContext;
        public ProductController(IProduct IProduct, BackgroundWorkerQueue backgroundWorkerQueue, BackgroundPriceQueue backgroundPriceQueue, BackgroundRemoveProductQueue backgroundRemoveProductQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _IProduct = IProduct;
            _backgroundWorkerQueue = backgroundWorkerQueue;
            _backgroundPriceQueue = backgroundPriceQueue;
            _backgroundRemoveProductQueue = backgroundRemoveProductQueue;
            _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        }
        [HttpGet("GetProductDetails/{categoryId}")]
        public async Task<List<product>> GetProductDetails(int categoryId)
        {
            return await Task.FromResult(_IProduct.GetProductDetails(categoryId));
        }
        [HttpPost("FindByFilter")]
        public async Task<ProductReturnData> FindByFilter([FromBody] ProductFilter productFilter)
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
        [HttpPost("AddImg")]
        public async Task<string> AddImg(ImageFile imageFile)
        {
            var buf = Convert.FromBase64String(imageFile.base64data);
            await System.IO.File.WriteAllBytesAsync($"{imageFile.fileName}", buf);
            return "";
        }
        [HttpGet("catalog")]
        public ActionResult Image()
        {

            var path = Path.Combine("123.jpg"); //validate the path for security or use other means to generate the path.
            return base.File(path, "image/jpeg");
        }
        [HttpPost("CreateProductFromSite")]
        public async Task<IActionResult> CreateProductFromSite(industriation_product industriation_Product)
        {

            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                Console.WriteLine($"Новый продукт в очереди {industriation_Product.product_id}");
                Log.Error($"Новый продукт в очереди {industriation_Product.product_id}");

                product? product = null;
                product = _dbContext.product.Where(p => p.external_id == industriation_Product.product_id).FirstOrDefault();

                if (product == null)
                    product = new();
                if (!String.IsNullOrEmpty(industriation_Product.manufacturer_name))
                    product.manufacturer = industriation_Product.manufacturer_name;
                if (!String.IsNullOrEmpty(industriation_Product.name))
                    product.name = industriation_Product.name;
                if (!String.IsNullOrEmpty(industriation_Product.model))
                    product.article = industriation_Product.model;
                if (industriation_Product.status == 0)
                    product.site_url = $"https://industriation.ru/index.php?road=product/product&product_id={industriation_Product.product_id}&CRM=kikikiku77721";
                else
                    product.site_url = $"https://industriation.ru/index.php?road=product/product&product_id={industriation_Product.product_id}";
                if (industriation_Product.quantity_class_id != null)
                {
                    if (industriation_Product.quantity_class_id == 1)
                        product.unit = "шт";
                    if (industriation_Product.quantity_class_id == 3)
                        product.unit = "м";
                }
                if (industriation_Product.main_category_id != null)
                {
                    var category = _dbContext.category.Where(c => c.id.ToString() == industriation_Product.main_category_id).FirstOrDefault();
                    if (category != null)
                        product.category_id = Convert.ToInt32(industriation_Product.main_category_id);
                }
                if (!String.IsNullOrEmpty(industriation_Product.price))
                {
                    double? price = 0;
                    try
                    {
                        price = Convert.ToDouble(industriation_Product.price?.Replace(',', '.')) * 1.2;
                        if (price != null)
                            price = Math.Round(price.Value);
                    }
                    catch
                    {
                        try
                        {
                            price = Convert.ToDouble(industriation_Product.price?.Replace('.', ',')) * 1.2;
                            if (price != null)
                                price = Math.Round(price.Value);
                        }
                        catch
                        {
                            price = null;
                        }
                    }
                    if (price != null)
                        product.price = price;
                }
                try
                {
                    if (product.id != null)
                    {
                        _dbContext.Entry(product).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        product.external_id = industriation_Product.product_id;
                        _dbContext.product.Add(product);
                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());

                }
                if (product.external_id != null)
                    RetailOrderCreator.CheckRetailProduct(product);

            }, industriation_Product.is_clone);
            return Ok();
        }

        [HttpPost("RemoveProductFromSite")]
        public async Task<IActionResult> RemoveProductFromSite(industriation_product industriation_Product)
        {
            Console.WriteLine($"Удаление продукта {industriation_Product.product_id}");

            _backgroundRemoveProductQueue.QueueBackgroundWorkItem(async token =>
            {
                Log.Error($"Удаление продукта {industriation_Product.product_id}");
                RetailOrderCreator.TryRemoveProduct(industriation_Product);
            });

            return Ok();
        }
        [HttpPost("UpdatePriceFromSite")]
        public async Task<IActionResult> UpdatePriceFromSite(price_model price_Model)
        {
            Console.WriteLine($"Обновление цены для {price_Model.product_id} - {price_Model.price}");

            _backgroundPriceQueue.QueueBackgroundWorkItem(async token =>
            {
                Log.Error($"Обновление цены для {price_Model.product_id} - {price_Model.price}");
                double? price = 0;
                try
                {
                    price = Convert.ToDouble(price_Model.price?.Replace(',', '.')) * 1.2;
                    if (price != null)
                        price = Math.Round(price.Value);
                }
                catch
                {
                    try
                    {
                        price = Convert.ToDouble(price_Model.price?.Replace('.', ',')) * 1.2;
                        if (price != null)
                            price = Math.Round(price.Value);
                    }
                    catch
                    {
                        price = null;
                    }
                }
                if (price != null)
                {
                    Console.WriteLine($"Цена для продукта {price_Model.product_id} - {price}");
                    try
                    {
                        var product = _dbContext.product.Where(p => p.external_id == price_Model.product_id).FirstOrDefault();

                        if (product != null)
                        {

                            product.price = price;
                            _dbContext.Entry(product).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());

                    }
                    RetailOrderCreator.updatePrice(price_Model.product_id, price.ToString());
                }
            });
            return Ok();
        }

    }
}
