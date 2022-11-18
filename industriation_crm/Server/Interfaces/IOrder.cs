using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IOrder
    {
        public OrdersReturnData GetOrderDetails(OrdersFilter ordersFilter);
        public order GetOrderData(int id);
        public int AddOrder(order order);
        public void UpdateOrderDetails(order order);
        public void DeleteOrder(int id);
    }
}
