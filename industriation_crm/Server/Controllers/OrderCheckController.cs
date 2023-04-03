using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCheckController : ControllerBase
    {
        private readonly IOrderCheck _IOrderCheck;
        public OrderCheckController(IOrderCheck IOrderCheck)
        {
            _IOrderCheck = IOrderCheck;
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IOrderCheck.RemoveOrderCheck(id);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update(order_check order_Check)
        {
            _IOrderCheck.UpdateOrderCheck(order_Check);
            return Ok();
        }
    }
}
