using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ITask
    {
        public List<task> GetUserTasks(int user_id);
        public List<task> GetTasksByClientId(int clientId);
        public int AddNewTask(task task);
        public void UpdateTask(task task);
    }
}
