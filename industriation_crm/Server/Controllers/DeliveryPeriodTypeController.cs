using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPeriodTypeController : ControllerBase
    {
        private readonly IDeliveryPeriodType _IDeliveryPeriodType;
        public DeliveryPeriodTypeController(IDeliveryPeriodType IDeliveryPeriodType)
        {
            _IDeliveryPeriodType = IDeliveryPeriodType;
        }
        [HttpGet]
        public async Task<List<delivery_period_type>> Get()
        {
            return await Task.FromResult(_IDeliveryPeriodType.GetDeliveryPeriodTypeDetails());
        }
    }
}
