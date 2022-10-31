using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IOrder
    {
        public List<order> GetOrderDetails();
        public order GetOrderData(int id);
        public int AddOrder(order order);
        public void UpdateOrderDetails(order order);
        public void DeleteOrder(int id);
    }
}
