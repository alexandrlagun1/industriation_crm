using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class TaskManager : ITask
    {
        readonly DatabaseContext _dbContext = new();
        public TaskManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<task> GetUserTasks(int user_id)
        {
            try
            {
                List<task> tasks = _dbContext.task.Where(t => t.executor_id == user_id).Include(t=>t.creator).ToList();
                return tasks;
            }
            catch
            {
                throw;
            }
        }
    }
}
