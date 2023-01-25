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

        public int AddNewTask(task task)
        {
            task.executor = null;
            task.creator = null;
            try
            {
                _dbContext.task.Add(task);
                _dbContext.SaveChanges();
                return task.id;
            }
            catch
            {
                throw;
            }
        }

        public List<task> GetTasksByClientId(int clientId)
        {
            try
            {
                List<task> tasks = _dbContext.task.Where(t => t.client_id == clientId).Include(t => t.creator).Include(t=>t.executor).ToList();
                return tasks;
            }
            catch
            {
                throw;
            }
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

        public void UpdateTask(task task)
        {
            try
            {
                _dbContext.Entry(task).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
