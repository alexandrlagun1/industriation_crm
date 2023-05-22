using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Duende.IdentityServer.Models.IdentityResources;

namespace industriation_crm.Server.Services
{
    public class UserManager : IUser
    {
        readonly DatabaseContext _dbContext = new();
        public UserManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<user> GetUserDetails(string trigger)
        {
            try
            {

                List<user> users = new List<user>();
                if (trigger == "all")
                    users = _dbContext.user.Include(c => c.roles)/*.Include(c => c.clients)*/.ToList();
                if (trigger == "managers")
                    users = _dbContext.user.Include(c => c.roles)/*.Include(c => c.clients)*/.Where(u => u.roles.id == 1).ToList();
                if (trigger == "suppliers")
                    users = _dbContext.user.Include(c => c.roles)/*.Include(c => c.clients)*/.Where(u => u.roles.id == 6).ToList();

                return users;
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(user user)
        {
            try
            {
                _dbContext.user.Add(user);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void UpdateUserDetails(user user)
        {
            try
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public user GetUserData(int id)
        {
            try
            {
                user? user = _dbContext.user.Find(id);
                if (user != null)
                {
                    return user;
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
        public void DeleteUser(int id)
        {
            try
            {
                user? user = _dbContext.user.Find(id);
                if (user != null)
                {
                    _dbContext.user.Remove(user);
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

        public user UserLogin(user _user)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.password == _user.password && u.login == _user.login).FirstOrDefault();
                if (user != null)
                {
                    return user;
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

        public user? GetUserDataByPhone(string phone)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.phone == phone).FirstOrDefault();
                return user;
            }
            catch
            {
                throw;
            }
        }

        public void SaveOrdersFilter(OrdersFilter ordersFilter)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(ordersFilter);
            try
            {
                user? user = _dbContext.user.Where(u => u.id == ordersFilter.user_id).FirstOrDefault();
                if (user != null)
                {
                    user.orders_filter = json;
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public OrdersFilter GetOrdersFilter(int user_id)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.id == user_id).FirstOrDefault();
                OrdersFilter ordersFilter = new();
                if (String.IsNullOrEmpty(user.orders_filter) == false)
                    ordersFilter = System.Text.Json.JsonSerializer.Deserialize<OrdersFilter>(user.orders_filter);
                return ordersFilter;
            }
            catch
            {
                throw;
            }
        }

        public void SaveClientsFilter(ClientFilter clientFilter)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(clientFilter);
            try
            {
                user? user = _dbContext.user.Where(u => u.id == clientFilter.user_id).FirstOrDefault();
                if (user != null)
                {
                    user.clients_filter = json;
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public ClientFilter GetClientsFilter(int user_id)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.id == user_id).FirstOrDefault();
                ClientFilter clientFilter = new();
                if (String.IsNullOrEmpty(user.clients_filter) == false)
                    clientFilter = System.Text.Json.JsonSerializer.Deserialize<ClientFilter>(user.clients_filter);
                return clientFilter;
            }
            catch
            {
                throw;
            }
        }

        public void SaveProductsFilter(ProductFilter productFilter)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(productFilter);
            try
            {
                user? user = _dbContext.user.Where(u => u.id == productFilter.user_id).FirstOrDefault();
                if (user != null)
                {
                    user.products_filter = json;
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public ProductFilter GetProductsFilter(int user_id)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.id == user_id).FirstOrDefault();
                ProductFilter productFilter = new();
                if (String.IsNullOrEmpty(user.products_filter) == false)
                    productFilter = System.Text.Json.JsonSerializer.Deserialize<ProductFilter>(user.products_filter);
                return productFilter;
            }
            catch
            {
                throw;
            }
        }

        public void SaveCallHistoryFilter(CallHistoryFilter callHistoryFilter)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(callHistoryFilter);
            try
            {
                user? user = _dbContext.user.Where(u => u.id == callHistoryFilter.user_id).FirstOrDefault();
                if (user != null)
                {
                    user.call_history_filter = json;
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public CallHistoryFilter GetCallHistoryFilter(int user_id)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.id == user_id).FirstOrDefault();
                CallHistoryFilter callHistoryFilter = new();
                if (String.IsNullOrEmpty(user.call_history_filter) == false)
                    callHistoryFilter = System.Text.Json.JsonSerializer.Deserialize<CallHistoryFilter>(user.call_history_filter);
                return callHistoryFilter;
            }
            catch
            {
                throw;
            }
        }
    }
}

