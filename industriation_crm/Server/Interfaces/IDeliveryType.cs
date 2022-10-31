using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IDeliveryType
    {
        public List<delivery_type> GetDeliveryTypeDetails();
    }
}
