using industriation_crm.Shared.Models;

namespace industriation_crm.Server.SignalRNotification
{
    public interface IStatusNotification
    {
        Task UpdateStatus(string status);
        Task MegafonCall(megafon_info megafon_info);
        Task AddNewPay(order_pay order_pay);
        Task UserTaskNotificate(string status);
    }
}
