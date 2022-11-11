using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
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

        public List<product> GetFromFilter(ProductFilter productFilter)
        {
            try
            {
                List<product> products = _dbContext.product.Where(p => p.price >= productFilter.price_from && p.price <= productFilter.price_to && p.article.Contains(productFilter.article) && p.name.Contains(productFilter.name)).Take(500).ToList();
                return products;
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
