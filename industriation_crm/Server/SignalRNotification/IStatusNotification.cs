namespace industriation_crm.Server.SignalRNotification
{
    public interface IStatusNotification
    {
        Task UpdateStatus(string status);
    }
}
