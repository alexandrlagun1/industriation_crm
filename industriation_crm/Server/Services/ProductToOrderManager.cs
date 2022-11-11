using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class ProductToOrderManager : IProductToOrder
    {
        readonly DatabaseContext _dbContext = new();
        public ProductToOrderManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void DeleteProductToOrderInRange(List<product_to_order> product_to_orders)
        {
            try
            {
                product_to_orders.ForEach(p => p.product = null);
                _dbContext.product_to_order.RemoveRange(product_to_orders);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void AddProductToOrderInRange(List<product_to_order> product_to_orders)
        {
            foreach (var p in product_to_orders)
                p.product = null;
            try
            {
                _dbContext.product_to_order.AddRange(product_to_orders);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public product_to_order AddProductToOrder(product_to_order product_to_order)
        {
            try
            {
                _dbContext.product_to_order.Add(product_to_order);
                _dbContext.SaveChanges();
                return product_to_order;
            }
            catch
            {
                throw;
            }
        }

        public void DeleteProductToOrder(int id)
        {
            try
            {
                product_to_order? product_to_order = _dbContext.product_to_order.Find(id);
                if (product_to_order != null)
                {
                    _dbContext.product_to_order.Remove(product_to_order);
                    _dbContext.SaveChanges();
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

        public List<product_to_order> GetProductsOfNoSupplierOrder()
        {
            try
            {
                return _dbContext.product_to_order.Include(p => p.product).Include(p => p.order).Where(p => p.supplier_order_id == null && p.order.order_status_id == 3).ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<product_to_order> GetProductsOfSupplierOrder(int suplier_order_id)
        {
            try
            {
                return _dbContext.product_to_order.Include(o => o.delivery_period_type).Include(p => p.product).Include(p => p.order).Where(p => p.supplier_order_id == suplier_order_id).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<product_to_order> GetProductToOrdertDetails(int order_id)
        {
            try
            {
                List<product_to_order> product_To_Order = _dbContext.product_to_order.Include(o => o.delivery_period_type).Include(p => p.product).Include(p => p.order).Where(p => p.order_id == order_id).ToList();
                return product_To_Order;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateProductsToOrder(List<product_to_order> product_to_orders)
        {
            foreach (var p in product_to_orders)
            {
                try
                {
                    _dbContext.Entry(p).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }


    }
}
