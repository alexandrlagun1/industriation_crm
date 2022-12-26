using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IUserNotifications
    {
        public List<user_notifications> GetUserNotifications(int user_id);
        public void AddUserNotifications(user_notifications user_notifications);
    }
}
