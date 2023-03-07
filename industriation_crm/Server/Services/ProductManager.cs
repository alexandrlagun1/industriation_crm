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
                int count = 0;
                if (productFilter.category_id != 1)
                {
                    count = _dbContext.product.Where(p => productFilter.child_categories.Contains(p.category_id) && p.price >= productFilter.price_from && p.price <= productFilter.price_to && p.article.Contains(productFilter.article) && p.name.Contains(productFilter.name)).Count();
                    products = _dbContext.product.Where(p => productFilter.child_categories.Contains(p.category_id) && p.price >= productFilter.price_from && p.price <= productFilter.price_to && p.article.Contains(productFilter.article) && p.name.Contains(productFilter.name)).Skip(productFilter.product_on_page * (productFilter.current_page - 1)).Take(productFilter.product_on_page).ToList();
                }
                else
                {
                    count = _dbContext.product.Where(p =>  p.price >= productFilter.price_from && p.price <= productFilter.price_to && p.article.Contains(productFilter.article) && p.name.Contains(productFilter.name)).Count();
                    products = _dbContext.product.Where(p =>  p.price >= productFilter.price_from && p.price <= productFilter.price_to && p.article.Contains(productFilter.article) && p.name.Contains(productFilter.name)).Skip(productFilter.product_on_page * (productFilter.current_page - 1)).Take(productFilter.product_on_page).ToList();
                }
                productReturnData.count = count;
                productReturnData.products = products.ToList();

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
                _dbContext.Entry(product).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
