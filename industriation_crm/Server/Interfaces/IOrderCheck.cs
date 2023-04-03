using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IOrderCheck
    {
        public void RemoveOrderCheck(int id);
        public void UpdateOrderCheck(order_check order_check);
    }
}
