using AutoMapper.Internal;
using industriation_crm.Server.Controllers._1C;
using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace industriation_crm.Server.Services
{
    public class OrderManager : IOrder
    {
        readonly DatabaseContext _dbContext = new();
        public OrderManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        private void UpdateInnerProductData(List<order_check>? order_Checks, int order_id)
        {
            foreach (var c in order_Checks)
            {
                List<product_to_order> product_To_Orders = new List<product_to_order>();
                product_To_Orders.AddRange(c.product_To_Orders);
                product_To_Orders.ForEach(p => p.product = null);
                product_To_Orders.ForEach(p => p.order_check = null);
                if (c.is_add == true)
                {
                    c.order_id = order_id;
                    _dbContext.order_check.Add(c);
                    _dbContext.SaveChanges();
                }
                if (c.id != 0 && c.is_delete == false)
                {
                    _dbContext.Entry(c).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                foreach (var p in product_To_Orders)
                {
                    if (p.order_check_id != 0)
                    {
                        _dbContext.Entry(p).State = EntityState.Modified;
                    }
                    if (p.is_add_from_order && p.order_check_id == 0)
                    {
                        p.order_check_id = c.id;
                        _dbContext.Add(p);
                    }
                    if (p.is_delete_from_order && p.order_check_id != 0)
                    {
                        _dbContext.Remove(p);
                    }

                    _dbContext.SaveChanges();
                }
            }
        }
        public int AddOrder(order order)
        {
            order.main_contact = null;
            try
            {
                List<order_check>? order_Checks = new List<order_check>();
                order_Checks.AddRange(order?.order_Checks!);

                if (order?.delivery?.delivery_type != null)
                    order.delivery.delivery_type = null;

                order.order_Checks = null;

                _dbContext.order.Add(order);
                _dbContext.SaveChanges();
                UpdateInnerProductData(order_Checks, order.id);
                return order.id;
            }
            catch
            {
                throw;
            }
        }
        public void UpdateOrderDetails(order order, bool is_update_inner_data)
        {
            try
            {
                _dbContext.Entry(order).State = EntityState.Modified;
                if (order.delivery_id != 0)
                    _dbContext.Entry(order.delivery!).State = EntityState.Modified;
                foreach (var task in order.tasks)
                {
                    if (task.id != 0)
                        _dbContext.Entry(task).State = EntityState.Modified;
                    else
                    {
                        task.creator = null;
                        task.executor = null;
                        _dbContext.task.Add(task);
                    }
                }
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
                if (is_update_inner_data)
                    UpdateInnerProductData(order?.order_Checks!, order.id);
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

                order? order = _dbContext.order.Include(o => o.order_Checks).ThenInclude(c => c.product_To_Orders).ThenInclude(p => p.product).Include(o => o.tasks).Include(o => o.order_Pays).Include(o => o.delivery).ThenInclude(d => d.delivery_type).Include(o => o.user).Include(o => o.client).ThenInclude(c => c.contacts).ThenInclude(c => c.contact_phones).Include(o => o.order_status).Include(o => o.main_contact).Include(o => o.order_Histories).Include(o => o.stage).FirstOrDefault(u => u.id == id);
                // order.product_To_Orders.ForEach((p) => { p.order_percent_discount = order._percent_discount; p.order_ruble_discount = order._ruble_discount; });
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

        public OrdersReturnData GetOrderDetails(OrdersFilter ordersFilter)
        {
            OrdersReturnData ordersReturnData = new OrdersReturnData();
            try
            {
                var query = _dbContext.order.Where(o => o.stage_id >= ordersFilter.stage && ordersFilter.pay_status.Contains(o.pay_status) && ordersFilter.managers.Contains(o.user) && ordersFilter.order_status.Contains(o.order_status));
                if(!String.IsNullOrEmpty(ordersFilter.product_article))
                    query = query.Where(o => o.order_Checks.Where(c => c.product_To_Orders.Select(p => p.product).Where(p => p.article.Contains(ordersFilter.product_article)).FirstOrDefault() != null).FirstOrDefault() != null);
                if (ordersFilter.client != null)
                    query = query.Where(o=>o.main_contact.full_name.Contains(ordersFilter.client));
                if (ordersFilter.pay_from != null)
                    query = query.Where(o => o.order_Pays.Where(p => p.date >= ordersFilter.pay_from).FirstOrDefault() != null);
                if (ordersFilter.order_id != null)
                    query = query.Where(o => o.id == ordersFilter.order_id);
                if(ordersFilter.order_date_from != null)
                    query = query.Where(o => o.order_date >= ordersFilter.order_date_from);
                if (ordersFilter.order_date_to != null)
                    query = query.Where(o => o.order_date <= ordersFilter.order_date_to);
                if(ordersFilter.delivey_from != null)
                    query = query.Where(o => o.delivery.shipment_date >= ordersFilter.delivey_from);
                if (ordersFilter.delivey_to != null)
                    query = query.Where(o => o.delivery.shipment_date <= ordersFilter.delivey_to);
                if (!String.IsNullOrEmpty(ordersFilter.client_email))
                    query = query.Where(o => o.client.contacts.Select(c => c.email).Contains(ordersFilter.client_email));
                ordersReturnData.count = query.Count();
                ordersReturnData.orders = query
                    .Include(o => o.user).Include(o => o.client).Include(o => o.order_status).Include(o => o.main_contact).Include(o => o.stage).Include(o => o.supplier_manager).Include(o => o.pay_status)
                    .OrderByDescending(o => o.order_date)
                    .Skip(ordersFilter.order_on_page * (ordersFilter.current_page - 1)).Take(ordersFilter.order_on_page).ToList();

                foreach (var o in ordersReturnData.orders)
                {
                    if (o.client != null)
                    {
                        o.client.user = null;
                        o.client.orders = null;
                    }
                    if (o.user != null)
                        o.user.clients = null;
                    if (o.main_contact != null)
                        o.main_contact.client = null;
                }
                return ordersReturnData;
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

        public List<order> GetOrdersByClientId(int clientId)
        {
            try
            {
                List<order> orders = _dbContext.order.Where(o => o.client_id == clientId).Include(o => o.order_Checks).ThenInclude(o => o.product_To_Orders).ThenInclude(p => p.product).ToList(); ;
                return orders;

            }
            catch
            {
                throw;
            }
        }
    }
}
