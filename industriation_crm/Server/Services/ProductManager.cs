using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
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

        public List<product> GetProductDetails()
        {
            try
            {
                return _dbContext.product.ToList();
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
