using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
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
        public int AddSupplierOrder(supplier_order supplier_order)
        {
            try
            {
                List<product_to_order> product_To_Orders = new List<product_to_order>();
                product_To_Orders.AddRange(supplier_order.product_to_orders);
                supplier_order.product_to_orders = null;
                supplier_order.supplier = null;
                _dbContext.supplier_order.Add(supplier_order);
                _dbContext.SaveChanges();

                foreach (var p in product_To_Orders)
                {
                    p.supplier_order_id = supplier_order.id;
                    _dbContext.Entry(p).State = EntityState.Modified;
                }
                _dbContext.SaveChanges();
                return supplier_order.id;
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
                supplier_order? supplier_order = _dbContext.supplier_order.Include(s => s.product_to_orders).ThenInclude(p => p.product).Include(s => s.product_to_orders).ThenInclude(p => p.order).Include(p => p.supplier).ThenInclude(o => o.contacts).FirstOrDefault(p => p.id == id);
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

        public SupplierOrderReturnData GetSupplierOrderDetails(SupplierOrderFilter ordersFilter)
        {
            SupplierOrderReturnData supplierOrderReturnData = new SupplierOrderReturnData();
            if (ordersFilter.order_date_from == null)
                ordersFilter.order_date_from = DateTime.MinValue;
            if (ordersFilter.order_date_to == null)
                ordersFilter.order_date_to = DateTime.MaxValue;
            try
            {
                if (ordersFilter.supplier_order_id == null) {
                    supplierOrderReturnData.count = _dbContext.supplier_order.Where(o => o.date >= ordersFilter.order_date_from && o.date <= ordersFilter.order_date_to && o.supplier.org_name.Contains(ordersFilter.supplier)).Count();
                    supplierOrderReturnData.supplier_orders = _dbContext.supplier_order.Where(o => o.date >= ordersFilter.order_date_from && o.date <= ordersFilter.order_date_to && o.supplier.org_name.Contains(ordersFilter.supplier))
                        .Include(s => s.product_to_orders).ThenInclude(p => p.product).Include(s => s.supplier).Include(s => s.user).Include(s=>s.supplier_order_status)
                        .OrderByDescending(o => o.date).Skip(ordersFilter.order_on_page * (ordersFilter.current_page - 1)).Take(ordersFilter.order_on_page).ToList();
                }
                else
                {
                    supplierOrderReturnData.count = _dbContext.supplier_order.Where(o =>o.id == ordersFilter.supplier_order_id && o.date >= ordersFilter.order_date_from && o.date <= ordersFilter.order_date_to && o.supplier.org_name.Contains(ordersFilter.supplier)).Count();
                    supplierOrderReturnData.supplier_orders = _dbContext.supplier_order.Where(o => o.id == ordersFilter.supplier_order_id && o.date >= ordersFilter.order_date_from && o.date <= ordersFilter.order_date_to && o.supplier.org_name.Contains(ordersFilter.supplier))
                        .Include(s => s.product_to_orders).ThenInclude(p => p.product).Include(s => s.supplier).Include(s => s.user).Include(s => s.supplier_order_status)
                        .OrderByDescending(o => o.date).Skip(ordersFilter.order_on_page * (ordersFilter.current_page - 1)).Take(ordersFilter.order_on_page).ToList();
                }
                return supplierOrderReturnData;
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
                foreach (var p in supplier_order.product_to_orders)
                {
                    p.supplier_order = null;
                    if (p.supplier_order_id != 0 && p.supplier_order_id != null)
                    {
                        if (p.id_delete_from_supplier_order == true)
                        {
                            p.supplier_order_id = null;
                            p.supplier_delivery_period = null;
                            p.supplier_price = null;
                        }
                    }
                    else
                    {
                        if (p.id_delete_from_supplier_order == false)
                            p.supplier_order_id = supplier_order.id;
                    }
                    _dbContext.Entry(p).State = EntityState.Modified;
                }
                supplier_order.product_to_orders = new();
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
