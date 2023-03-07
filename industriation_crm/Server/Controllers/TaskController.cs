using industriation_crm.Server.Interfaces;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask _ITask;
        private readonly IHubContext<StatusNotificationHub, IStatusNotification> hubContext;
        public TaskController(ITask ITask, IHubContext<StatusNotificationHub, IStatusNotification> statusHub)
        {
            _ITask = ITask;
            this.hubContext = statusHub;
        }
        [HttpGet("{user_id}")]
        public async Task<List<task>> Get(int user_id)
        {
            return await Task.FromResult(_ITask.GetUserTasks(user_id));
        }
        [HttpGet("GetTasksByClientId/{client_id}")]
        public async Task<List<task>> GetTasksByClientId(int client_id)
        {
            return await Task.FromResult(_ITask.GetTasksByClientId(client_id));
        }
        [HttpPut]
        public void Put(task task)
        {
            _ITask.UpdateTask(task);
        }
        [HttpPost]
        public async Task<int> Post(task task)
        {
            int task_id = _ITask.AddNewTask(task);
            await this.hubContext.Clients.User(task.executor_id.ToString()).UserTaskNotificate("1");
            return task_id;
        }
    }
}
