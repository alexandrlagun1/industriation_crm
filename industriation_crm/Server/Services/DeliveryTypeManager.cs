using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class DeliveryTypeManager : IDeliveryType
    {
        readonly DatabaseContext _dbContext = new();
        public DeliveryTypeManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<delivery_type> GetDeliveryTypeDetails()
        {
            try
            {
                List<delivery_type> delivery_types = _dbContext.delivery_type.ToList();
                return delivery_types;
            }
            catch
            {
                throw;
            }
        }
    }
}
