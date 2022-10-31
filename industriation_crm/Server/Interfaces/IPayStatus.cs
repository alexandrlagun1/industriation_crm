using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IPayStatus
    {
        public List<pay_status> GetPayStatusDetails();
    }
}
