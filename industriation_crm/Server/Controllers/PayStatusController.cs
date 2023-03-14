using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

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
        public List<pay_status> Get()
        {
            List<pay_status> pay_Statuses = _IPayStatus.GetPayStatusDetails();
            return pay_Statuses;
        }
    }
}
