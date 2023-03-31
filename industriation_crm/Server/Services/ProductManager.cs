using Duende.IdentityServer.Extensions;
using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Server.Retail;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class ProductManager : IProduct
    {
        readonly DatabaseContext _dbContext = new();
        public ProductManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddProduct(product product)
        {
            try
            {
                _dbContext.product.Add(product);
                _dbContext.SaveChanges();
                product.external_id = product.id + 1000000;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public ProductReturnData GetFromFilter(ProductFilter productFilter)
        {
            try
            {
                ProductReturnData productReturnData = new ProductReturnData();
                List<product> products = new();
                var query = _dbContext.product.Where(p => p != null);
                if (!String.IsNullOrEmpty(productFilter.name))
                {
                    query = query.Where(p => p.name.Contains(productFilter.name));
                }
                if (!String.IsNullOrEmpty(productFilter.article))
                {
                    query = query.Where(p => p.article.Contains(productFilter.article));
                }
                if (productFilter.price_from != null && productFilter.price_from > 0)
                {
                    query = query.Where(p => p.price >= productFilter.price_from);
                }
                if (productFilter.price_to != null)
                {
                    query = query.Where(p => p.price <= productFilter.price_to);
                }
                if (productFilter.category_id != 1)
                {
                    query = query.Where(p => productFilter.child_categories.Contains(p.category_id));
                }
                productReturnData.count = query.Count();
                productReturnData.products = query.Skip(productFilter.product_on_page * (productFilter.current_page - 1)).Take(productFilter.product_on_page).ToList();

                return productReturnData;
            }
            catch
            {
                throw;
            }
        }



        public product GetProductData(int id)
        {
            try
            {
                product? product = _dbContext.product.Find(id);
                if (product != null)
                {
                    return product;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<product> GetProductDetails(int categoryId)
        {
            try
            {
                List<product> products = _dbContext.product.Where(p => p.category_id == categoryId).ToList();
                return products;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateProductDetails(product product)
        {
            try
            {
                if (product.id != null)
                {
                    _dbContext.Entry(product).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                else
                {
                    var db_product = _dbContext.product.Where(p => p.external_id == product.external_id).FirstOrDefault();
                    if (db_product != null)
                    {
                        if (product.price != null)
                            db_product.price = product.price;
                        if (product.quantity != null)
                            db_product.quantity = product.quantity;
                        if (product.article != null)
                            db_product.article = product.article;
                        if (product.manufacturer != null)
                            db_product.manufacturer = product.manufacturer;
                        if (product.name != null)
                            db_product.name = product.name;
                        _dbContext.Entry(db_product).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
