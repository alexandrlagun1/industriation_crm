using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class OrderCheckManager : IOrderCheck
    {
        readonly DatabaseContext _dbContext = new();
        public OrderCheckManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void RemoveOrderCheck(int id)
        {
            try
            {
                var order_check = _dbContext.order_check.Include(o => o.order).Include(o=>o.product_To_Orders).Where(c=>c.id == id).FirstOrDefault();
                
                if (order_check != null)
                {
                    _dbContext.product_to_order.RemoveRange(order_check.product_To_Orders);
                    _dbContext.order_check.Remove(order_check);
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

        public void UpdateOrderCheck(order_check order_check)
        {
            try
            {
                _dbContext.Entry(order_check).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
