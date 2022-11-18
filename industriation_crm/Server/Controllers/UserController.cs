using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _IUser;
        public UserController(IUser iUser)
        {
            _IUser = iUser;
        }
        [HttpGet]
        public async Task<List<user>> Get(string type)
        {
            return await Task.FromResult(_IUser.GetUserDetails(type));
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            user user = _IUser.GetUserData(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
        [HttpPost]
        public void Post(user user)
        {
            _IUser.AddUser(user);
        }
        [HttpPut]
        public void Put(user user)
        {
            _IUser.UpdateUserDetails(user);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IUser.DeleteUser(id);
            return Ok();
        }
    }
}
