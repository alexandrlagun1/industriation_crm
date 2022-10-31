using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayStatusController : ControllerBase
    {
        private readonly IPayStatus _IPayStatus;
        public PayStatusController(IPayStatus IPayStatus)
        {
            _IPayStatus = IPayStatus;
        }
        [HttpGet]
        public async Task<List<pay_status>> Get()
{
            return await Task.FromResult(_IPayStatus.GetPayStatusDetails());
        }
    }
}
