using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Services
{
    public class UserNotificationsManager : IUserNotifications
    {
        readonly DatabaseContext _dbContext = new();
        public UserNotificationsManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddUserNotifications(user_notifications user_notifications)
        {
            try
            {
                _dbContext.user_notifications.Add(user_notifications);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<user_notifications> GetUserNotifications(int user_id)
        {
            return _dbContext.user_notifications.Where(n => n.user_id == user_id).ToList();
        }

    }
}
