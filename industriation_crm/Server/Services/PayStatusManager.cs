using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Services
{
    public class PayStatusManager : IPayStatus
    {
        readonly DatabaseContext _dbContext = new();
        public PayStatusManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<pay_status> GetPayStatusDetails()
        {
            List<pay_status> pay_Statuses = _dbContext.pay_status.ToList();
            return pay_Statuses;
        }
    }
}
