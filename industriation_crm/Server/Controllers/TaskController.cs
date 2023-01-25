using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask _ITask;
        public TaskController(ITask ITask)
        {
            _ITask = ITask;
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
        public int Post(task task)
        {
            return  _ITask.AddNewTask(task);
        }
    }
}
