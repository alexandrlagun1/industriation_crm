using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientDiscountController : ControllerBase
    {
        private readonly IProgressClientDiscount _IProgressClientDiscount;
        public ClientDiscountController(IProgressClientDiscount IProgressClientDiscount)
        {
            _IProgressClientDiscount = IProgressClientDiscount;
        }
        [HttpGet]
        public IActionResult Get()
        {
            progress_client_discount progress_client_discount = _IProgressClientDiscount.GetClientDiscount();
            if (progress_client_discount != null)
            {
                return Ok(progress_client_discount);
            }
            return NotFound();
        }
    }
}
