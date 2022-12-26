using industriation_crm.Server._1C;
using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNotificationsController : ControllerBase
    {
        private readonly IUserNotifications _IUserNotifications;
        public UserNotificationsController(IUserNotifications iUserNotifications)
        {
            _IUserNotifications = iUserNotifications;
        }
        [HttpGet("{user_id}")]
        public async Task<List<user_notifications>> Get(int user_id)
        {
            return await Task.FromResult(_IUserNotifications.GetUserNotifications(user_id));
        }
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    product product = _IProduct.GetProductData(id);
        //    if (product != null)
        //    {
        //        return Ok(product);
        //    }
        //    return NotFound();
        //}
        [HttpPost]
        public void Post(user_notifications user_notifications)
        {
            _IUserNotifications.AddUserNotifications(user_notifications);
        }
    }
}
