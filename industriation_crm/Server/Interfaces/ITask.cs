using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ITask
    {
        public List<task> GetUserTasks(int user_id);
    }
}
