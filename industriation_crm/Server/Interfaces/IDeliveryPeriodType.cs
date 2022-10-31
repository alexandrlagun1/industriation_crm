using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IDeliveryPeriodType
    {
        public List<delivery_period_type> GetDeliveryPeriodTypeDetails();
    }
}
