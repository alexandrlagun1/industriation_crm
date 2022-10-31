using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class DeliveryPeriodTypeManager : IDeliveryPeriodType
    {
        readonly DatabaseContext _dbContext = new();
        public DeliveryPeriodTypeManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<delivery_period_type> GetDeliveryPeriodTypeDetails()
        {
            try
            {
                List<delivery_period_type> delivery_period_types = _dbContext.delivery_period_type.ToList();
                return delivery_period_types;
            }
            catch
            {
                throw;
            }
        }
    }
}
