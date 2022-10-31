using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoles _IRoles;
        public RolesController(IRoles IRoles)
        {
            _IRoles = IRoles;
        }
        [HttpGet]
        public async Task<List<roles>> Get()
        {
            return await Task.FromResult(_IRoles.GetRolesDetails());
        }
    }
}
