using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class OrderHistoryManager : IOrderHistory
    {
        readonly DatabaseContext _dbContext = new();
        public OrderHistoryManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddOrderHistory(order_history order_history)
        {
            try
            {
                _dbContext.order_history.Add(order_history);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
