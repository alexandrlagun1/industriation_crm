using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IDeliveryType _IDeliveryType;
        public DeliveryTypeController(IDeliveryType iDeliveryType)
        {
            _IDeliveryType = iDeliveryType;
        }
        [HttpGet]
        public async Task<List<delivery_type>> Get()
        {
            List<delivery_type> delivery_types = await Task.FromResult(_IDeliveryType.GetDeliveryTypeDetails());
            return delivery_types;

        }
    }
}
