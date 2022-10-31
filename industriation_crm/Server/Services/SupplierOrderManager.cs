using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class SupplierOrderManager : ISupplierOrder
    {
        readonly DatabaseContext _dbContext = new();
        public SupplierOrderManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddSupplierOrder(supplier_order supplier_order)
        {
            try
            {
                _dbContext.supplier_order.Add(supplier_order);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSupplierOrder(int id)
        {

            try
            {
                supplier_order? supplier_order = _dbContext.supplier_order.Include(s => s.product_to_orders).Where(s => s.id == id).FirstOrDefault();
                if (supplier_order != null)
                {
                    _dbContext.supplier_order.Remove(supplier_order);
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

        public supplier_order GetSupplierOrderData(int id)
        {
            try
            {
                supplier_order? supplier_order = _dbContext.supplier_order.Find(id);
                if (supplier_order != null)
                {
                    return supplier_order;
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

        public List<supplier_order> GetSupplierOrderDetails()
        {
            try
            {
                List<supplier_order> supplier_Orders = _dbContext.supplier_order.Include(s => s.supplier).Include(s => s.product_to_orders).ToList();
                return supplier_Orders;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateSupplierOrderDetails(supplier_order supplier_order)
        {
            try
            {
                _dbContext.Entry(supplier_order).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
