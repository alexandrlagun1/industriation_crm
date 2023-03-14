using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Services
{
    public class OrderStatusManager : IOrderStatus
    {
        readonly DatabaseContext _dbContext = new();
        public OrderStatusManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<order_status> GetOrderStatusDetails()
        {
            List<order_status> order_Statuses = _dbContext.order_status.ToList();
            return order_Statuses;
        }
    }
}
