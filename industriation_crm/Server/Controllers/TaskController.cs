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
    }
}
