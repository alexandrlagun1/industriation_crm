using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class OrderManager : IOrder
    {
        readonly DatabaseContext _dbContext = new();
        public OrderManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddOrder(order order)
        {
            order.main_contact = null;
            try
            {
                if (order?.product_To_Orders != null)
                    foreach (var p in order?.product_To_Orders)
                    {
                        p.product = null;
                    }
                if (order?.delivery?.delivery_type != null)
                    order.delivery.delivery_type = null;
                order.product_To_Orders = null;
                _dbContext.order.Add(order);
                _dbContext.SaveChanges();
                return order.id;
            }
            catch
            {
                throw;
            }
        }
        public void UpdateOrderDetails(order order)
        {
            try
            {
                _dbContext.Entry(order).State = EntityState.Modified;
                if (order.delivery_id != 0)
                    _dbContext.Entry(order.delivery!).State = EntityState.Modified;
                foreach (var pay in order.order_Pays)
                {
                    if (pay.id != 0)
                    {
                        _dbContext.Entry(pay).State = EntityState.Modified;
                    }
                    else
                    {
                        pay.order_id = order.id;
                        _dbContext.order_pay.Add(pay);
                    }
                    if (pay.isRemove == true)
                    {
                        _dbContext.order_pay.Remove(pay);
                    }
                }

                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public order GetOrderData(int id)
        {
            try
            {
                order? order = _dbContext.order.Include(o => o.order_Pays).ThenInclude(p => p.pay_Status).Include(o => o.delivery).ThenInclude(d => d.delivery_type).Include(o => o.user).Include(o => o.client).ThenInclude(c => c.contacts).ThenInclude(c=>c.contact_phones).Include(o => o.order_status).Include(o => o.product_To_Orders).ThenInclude(o => o.product).Include(o => o.main_contact).FirstOrDefault(u => u.id == id);
                if (order != null)
                {
                    return order;
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

        public List<order> GetOrderDetails()
        {
            try
            {
                List<order> orders = _dbContext.order.Include(o => o.user).Include(o => o.client).Include(o => o.order_status).ToList();
                return orders;
            }
            catch
            {
                throw;
            }
        }

        public void DeleteOrder(int id)
        {
            try
            {
                order? order = _dbContext.order.Find(id);
                if (order != null)
                {
                    _dbContext.order.Remove(order);
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
    }
}
