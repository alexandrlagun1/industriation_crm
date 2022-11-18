using AutoMapper.Internal;
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
                order? order = _dbContext.order.Include(o => o.order_Pays).ThenInclude(p => p.pay_Status).Include(o => o.delivery).ThenInclude(d => d.delivery_type).Include(o => o.user).Include(o => o.client).ThenInclude(c => c.contacts).ThenInclude(c => c.contact_phones).Include(o => o.order_status).Include(o => o.product_To_Orders).ThenInclude(o => o.product).Include(o => o.main_contact).FirstOrDefault(u => u.id == id);
                order.product_To_Orders.ForEach((p) => { p.order_percent_discount = order._percent_discount; p.order_ruble_discount = order._ruble_discount; });
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
            if (ordersFilter.order_date_from == null)
                ordersFilter.order_date_from = DateTime.MinValue;
            if (ordersFilter.order_date_to == null)
                ordersFilter.order_date_to = DateTime.MaxValue;
            try
            {
                if (ordersFilter.order_id == null || ordersFilter.order_id == "")
                {
                    ordersReturnData.count = _dbContext.order.Where(o => ordersFilter.managers.Contains(o.user) && o.main_contact.full_name.Contains(ordersFilter.client) && o.order_date >= ordersFilter.order_date_from && o.order_date <= ordersFilter.order_date_to).Count();

                    ordersReturnData.orders = _dbContext.order.Where(o => ordersFilter.managers.Contains(o.user) && o.main_contact.full_name.Contains(ordersFilter.client) && o.order_date >= ordersFilter.order_date_from && o.order_date <= ordersFilter.order_date_to)
                        .Include(o => o.user).Include(o => o.client).Include(o => o.order_status).Include(o=>o.main_contact)
                        .Skip(ordersFilter.client_on_page * (ordersFilter.current_page - 1)).Take(ordersFilter.client_on_page).ToList();
                }
                else
                {
                    ordersReturnData.count = _dbContext.order.Where(o => o.id == Convert.ToInt32(ordersFilter.order_id) && ordersFilter.managers.Contains(o.user) && o.main_contact.full_name.Contains(ordersFilter.client) && o.order_date >= ordersFilter.order_date_from && o.order_date <= ordersFilter.order_date_to).Count();

                    ordersReturnData.orders = _dbContext.order.Where(o => o.id == Convert.ToInt32(ordersFilter.order_id) && ordersFilter.managers.Contains(o.user) && o.main_contact.full_name.Contains(ordersFilter.client) && o.order_date >= ordersFilter.order_date_from && o.order_date <= ordersFilter.order_date_to)
                        .Include(o => o.user).Include(o => o.client).Include(o => o.order_status).Include(o => o.main_contact)
                        .Skip(ordersFilter.client_on_page * (ordersFilter.current_page - 1)).Take(ordersFilter.client_on_page).ToList();
                }
                foreach(var o in ordersReturnData.orders)
                {
                    if(o.client != null)
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
    }
}
