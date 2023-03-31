using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IUser
    {
        public List<user> GetUserDetails(string trigger);
        public void AddUser(user user);
        public void UpdateUserDetails(user user);
        public user GetUserData(int id);
        public user? GetUserDataByPhone(string phone);
        public void DeleteUser(int id);
        public user UserLogin(user user);
        public void SaveOrdersFilter(OrdersFilter ordersFilter);
        public OrdersFilter GetOrdersFilter(int user_id);
        public void SaveClientsFilter(ClientFilter clientFilter);
        public ClientFilter GetClientsFilter(int user_id);
        public void SaveProductsFilter(ProductFilter productFilter);
        public ProductFilter GetProductsFilter(int user_id);
        public void SaveCallHistoryFilter(CallHistoryFilter callHistoryFilter);
        public CallHistoryFilter GetCallHistoryFilter(int user_id);
    }
}
