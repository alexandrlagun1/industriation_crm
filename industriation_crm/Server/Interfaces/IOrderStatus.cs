using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IOrderStatus
    {
        public List<order_status> GetOrderStatusDetails();
    }
}
